namespace LCPC.Domain.Entities;

public enum ChannelType
{
    /// <summary>
    /// 供应商进货
    /// </summary>
    SUPILER = 0,

    /// <summary>
    /// 自行采购
    /// </summary>
    CUSTOMER = 1
}

public enum InOStatus
{
    /// <summary>
    /// 全部
    /// </summary>
    ALL = -1,
    /// <summary>
    /// 进行中
    /// </summary>
    APAY = 1,

    /// <summary>
    /// 已完成
    /// </summary>
    EPAY = 2
    
}


/// <summary>
/// 进货单
/// </summary>
public class PurchaseInOrder:EntityBase
{
    /// <summary>
    /// 采购编码
    /// </summary>
    public string PurchaseCode { get; set; }
    /// <summary>
    /// 进货日期
    /// </summary>
    public string InOrderTime { get; set; }

    /// <summary>
    /// 供货渠道
    /// </summary>
    public ChannelType ChannelType { get; set; }

    /// <summary>
    /// 物流单号
    /// </summary>
    public string Logistics { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string InUser { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string InPhone { get; set; }

    /// <summary>
    /// 供应商主键
    /// </summary>
    public string SupplierId { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public virtual SupplierInfo SupplierInfo { get; set; }

    /// <summary>
    /// 进货总数
    /// </summary>
    public int InCount { get; set; }

    /// <summary>
    /// 进货总价
    /// </summary>
    public decimal InPrice { get; set; }

    /// <summary>
    /// 进货单状态
    /// </summary>
    public InOStatus InOStatus { get; set; }

   

    public virtual ICollection<PurchaseInDetail> PurchaseInDetails { get; set; } = new List<PurchaseInDetail>();
}