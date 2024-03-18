namespace LCPC.Domain.Entities
{
    public enum OrderStatus
    {
        /// <summary>
        /// 全部查询
        /// </summary>
        ALL = -1,
        /// <summary>
        /// 待支付
        /// </summary>
        AWAIT = 0,
        /// <summary>
        /// 取消
        /// </summary>
        CANCLE = 9,
        /// <summary>
        /// 已支付
        /// </summary>
        PAYEND = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        COMPLETE = 2,
        /// <summary>
        /// 已作废，STATUS = 1
        /// </summary>
        DELETE = 3
    }
}