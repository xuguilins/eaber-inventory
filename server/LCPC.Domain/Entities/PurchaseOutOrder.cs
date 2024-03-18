namespace LCPC.Domain.Entities;

public enum OutStatus
{
    /// <summary>
    /// 退货中
    /// </summary>
    INING = 1,
    /// <summary>
    /// 已完成退货
    /// </summary>
    COMPLETE = 2
}

/// <summary>
/// 退货单
/// </summary>
public class PurchaseOutOrder:EntityBase
{
    /// <summary>
    /// 退货编码
    /// </summary>
    public string PurchaseCode { get; set; }
    /// <summary>
    /// 退货日期
    /// </summary>
    public string OrderTime { get; set; }
    /// <summary>
    /// 进货单据编码
    /// </summary>
    public string InOrderCode { get; set; }
    /// <summary>
    /// 供应商主键
    /// </summary>
    public string SupilerId { get; set; }
    public virtual SupplierInfo  SupplierInfo { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>
    public string InUser { get; set; }

    /// <summary>
    /// 退货物流单号
    /// </summary>
    public string Logicse { get; set; }

    /// <summary>
    /// 退货单总价
    /// </summary>
    public decimal OutOrderPrice { get; set; }

    /// <summary>
    /// 退货总数
    /// </summary>
    public int OutOrderCount { get; set; }
    /// <summary>
    /// 收货方联系电话
    /// </summary>
    public string InPhone { get; set; }

    public OutStatus OutStatus { get; set; }

    public virtual ICollection<PurchaseOutOrderDetail> PurashOutDetails { get; set; }
        = new List<PurchaseOutOrderDetail>();

}