using LCPC.Domain.Notifys;

namespace LCPC.Domain.CommandHandlers;

public class OrderConfirmCommandHandler : IRequestHandler<OrderConfirmCommand, ReturnResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IMediator _mediator;

    public OrderConfirmCommandHandler(IOrderRepository orderRepository, IProdcutRepository prodcutRepository,IMediator mediator)
    {
        _orderRepository = orderRepository;
        _prodcutRepository = prodcutRepository;
        _mediator = mediator;
    }

    public async Task<ReturnResult> Handle(OrderConfirmCommand request, CancellationToken cancellationToken)
    {
        var models = await _orderRepository.GetOrderInfo(request.Ids);
        if (!models.Any())
            throw new Exception("未找到有效的订单");
        if (request.OrderStatus == OrderStatus.CANCLE)
            await CancleOrder(models);
        if (request.OrderStatus == OrderStatus.DELETE)
            await DeleteOrder(models);
        if (request.OrderStatus == OrderStatus.PAYEND)
            await PayOrders(models,request.PayName);
        if (request.OrderStatus == OrderStatus.COMPLETE)
            await CompleteOrders(models);
        var result = await _orderRepository.UnitOfWork.SaveChangesAsync();
        if (result > 0)
            await _mediator.Publish(new OrderNotify());
        string message = string.Empty;
        if (request.OrderStatus == OrderStatus.CANCLE)
            message = "订单取消";
        if (request.OrderStatus == OrderStatus.DELETE)
            message = "订单作废";
        if (request.OrderStatus == OrderStatus.PAYEND)
            message = "订单确认支付";
        if (request.OrderStatus == OrderStatus.COMPLETE)
            message = "订单确认完成";
        return result > 0
            ? new ReturnResult(true, null, message+"成功")
            : new ReturnResult(false, null, message+"失败");
    }
    
    #region 订单完成/占用库存

    private async Task CompleteOrders(List<OrderInfo> orders)
    {
        foreach (var order in orders)
        {
            order.OrderStatus = OrderStatus.COMPLETE;
            await _orderRepository.UpdateAsync(order);
        }
    }

    #endregion

    #region 订单支付/占用库存

    private async Task PayOrders(List<OrderInfo> orders,string PayName)
    {
        foreach (var order in orders)
        {
            order.OrderStatus = OrderStatus.PAYEND;
            order.OrderPay = PayName;
            await _orderRepository.UpdateAsync(order);
        }
    }

    #endregion
    
    #region 取消订单/释放库存
    private async Task CancleOrder(List<OrderInfo> orders)
    {
        foreach (var order in orders)
        {
            order.OrderStatus = OrderStatus.CANCLE;
            await _orderRepository.UpdateAsync(order);
            await CallBackProduct(order.OrderInfoDetails.ToList());
        }
    }

    

    #endregion

    #region 订单作废/释放库存
    private async Task DeleteOrder(List<OrderInfo> orders)
    {
        foreach (var order in orders)
        {
           if(order.OrderStatus != OrderStatus.CANCLE)
               await CallBackProduct(order.OrderInfoDetails.ToList());
            order.OrderStatus = OrderStatus.DELETE;
            order.Enable = false;
            await _orderRepository.UpdateAsync(order);
        }
    }
    

    #endregion
   
    #region 释放商品
    private async Task CallBackProduct(List<OrderInfoDetail> details)
    {
        var ids = details.Select(x => x.ProductId).ToList();
        var products = await _prodcutRepository.GetTrackEntitiesAsync(x => ids.Contains(x.Id));
        for (int i = 0; i < details.Count; i++)
        {
            var item = details[i];
            var product = products.FirstOrDefault(x => x.Id == item.ProductId);
            if (product != null)
                product.InventoryCount += item.OrderCount;
            await _prodcutRepository.UpdateAsync(product);
        }
    }
    #endregion
    
}