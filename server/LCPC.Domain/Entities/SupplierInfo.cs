namespace LCPC.Domain.Entities;

public class SupplierInfo : EntityBase
{
    /// <summary>
    /// 供应商编码
    /// </summary>
    public string SupNumber { get; set; }
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupName { get; set; }
    /// <summary>
    /// 手机1
    /// </summary>
    public string SupTel { get; set; }
    /// <summary>
    /// 手机号2
    /// </summary>
    public string SupTelT { get; set; }
    /// <summary>
    /// 座机
    /// </summary>
    public string SupPhone { get; set; }
    /// <summary>
    /// 座机2
    /// </summary>
    public string SupPhoneT { get; set; }
    /// <summary>
    /// 供应商联系人
    /// </summary>
    public string ProviderUser { get; set; }

    /// <summary>
    /// 供应商联系人2
    /// </summary>
    public string ProviderUserT { get; set; }

    /// <summary>
    /// 联系地址
    /// </summary>
    public string Address { get; set; }

    public virtual ICollection<ProductInfo>  ProductInfos  { get; set; }
}