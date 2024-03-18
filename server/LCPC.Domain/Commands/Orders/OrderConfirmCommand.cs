namespace LCPC.Domain.Commands;

public class OrderConfirmCommand:IRequest<ReturnResult>
{
    public string[] Ids { get;  set; }
    /// <summary>
    /// 9 取消
    /// 1 确认支付
    /// </summary>
    public OrderStatus  OrderStatus { get; set; }

    public string PayName { get; set; }
}