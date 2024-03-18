
namespace LCPC.Domain.Commands
{
    public class CreateOrderCommand : IRequest<ReturnResult>
    {
        public string SellTime { get; set; }
        public string SellUser { get; set; }
        public string SellPhone { get; set; }
        public string PayName { get; set; }
        
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal OffsetMoney { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ActuailMoney { get; set; }
        public List<OrderProduct> Products { get; set; } = new List<OrderProduct>();
        public string Remark { get; set; }
    }
    public class OrderProduct
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        /// <summary>
        /// 下单数量
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 订单单价
        /// </summary>
        public decimal OrderSigle { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal OrderPrice { get; set; }

        public string Remark { get; set; }
    }
}
