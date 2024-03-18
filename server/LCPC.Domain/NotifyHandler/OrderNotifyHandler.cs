using LCPC.Domain.Notifys;
using LCPC.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LCPC.Domain.NotifyHandler;

public class OrderNotifyHandler:INotificationHandler<OrderNotify>
{
    private readonly IHubContext<HubClient, IHubClient> _hubContext;
    private readonly IOrderRepository _orderRepository;
    public OrderNotifyHandler(IHubContext<HubClient, IHubClient>  hubContext,IOrderRepository orderRepository)
    {
        _hubContext = hubContext;
        _orderRepository = orderRepository;
    }
    public async  Task Handle(OrderNotify notification, CancellationToken cancellationToken)
    {
        
        var orders = _orderRepository.GetEntities.Select(x => new
        {
            Code = x.OrderCode,
            Status = x.OrderStatus
        }).ToList();
        OrderCountData info = new OrderCountData();
        info.AwaitCount = orders.Count(d => d.Status == OrderStatus.AWAIT);
        info.CompleteCount =  orders.Count(d => d.Status == OrderStatus.COMPLETE);
        info.CancleCount =  orders.Count(d => d.Status == OrderStatus.CANCLE);
        info.PayCount  =  orders.Count(d => d.Status == OrderStatus.PAYEND);
        await _hubContext.Clients.All.SendAll(info);
    }
}