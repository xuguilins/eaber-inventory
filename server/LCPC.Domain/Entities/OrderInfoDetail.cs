
namespace LCPC.Domain.Entities
{
    public class OrderInfoDetail : EntityBase
    {

        /// <summary>
        /// 订单主键
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品单位
        /// </summary>

        public string UnitName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
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

        /// <summary>
        /// 关联订单主键
        /// </summary>
        public OrderInfo Order { get; set; }
    }
}