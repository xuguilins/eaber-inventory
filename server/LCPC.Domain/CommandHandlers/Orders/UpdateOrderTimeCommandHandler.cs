namespace LCPC.Domain.CommandHandlers;

public class UpdateOrderTimeCommandHandler:IRequestHandler<UpdateOrderCommand,ReturnResult>
{
    private readonly IOrderRepository _orderRepository;
    public UpdateOrderTimeCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<ReturnResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByKey(request.Id);
        if (order == null)
            throw new Exception("无效的订单数据");
        order.OrderTime = request.Time;
        order.OrderPay = request.PayName;
        await _orderRepository.UpdateAsync(order);
        int result = await _orderRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "单据已更新")
            : new ReturnResult(false, null, "单据更新失败");
    }
}