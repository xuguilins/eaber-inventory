

using Microsoft.Identity.Client;

namespace LCPC.Domain.Entities
{
    public class OrderInfo : EntityBase
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 开单日期
        /// </summary>
        public string OrderTime { get; set; }
        /// <summary>
        /// 购买单位
        /// </summary>
        public string OrderUser { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string OrderTel { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string OrderPay { get; set; }
        /// <summary>
        /// 订单形成平台
        ///  pc/app
        /// </summary>
        public string OrderClient { get; set; } = "PC";

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderMoney { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal OffsetMoney { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ActuailMoney { get; set; }

        /// <summary>
        /// 购买单位主键
        /// </summary>
        public string OrderUserId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderInfoDetail> OrderInfoDetails { get; set; } = new List<OrderInfoDetail>();
    }
}