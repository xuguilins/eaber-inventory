namespace LCPC.Domain.Entities;

/// <summary>
/// 退货单明细
/// </summary>
public class PurchaseOutOrderDetail:EntityBase
{
    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// 型号
    /// </summary>
    public string ProductModel { get; set; }
    /// <summary>
    /// 进货数量
    /// </summary>
    public int InCount { get; set; }
    /// <summary>
    /// 进货价
    /// </summary>
    public decimal InPrice { get; set; }
    /// <summary>
    /// 退货数量
    /// </summary>
    public int OutCount { get; set; }
    /// <summary>
    /// 退货价格
    /// </summary>
    public decimal OutPrice { get; set; }

    /// <summary>
    /// 退货总价
    /// </summary>
    /// <returns></returns>
    public decimal OutAllPrice { get; set; }

    /// <summary>
    /// 关联主键Id
    /// </summary>
    public string PurchaseId { get; set; }
    /// <summary>
    /// 导航熟悉
    /// </summary>
    /// <returns></returns>
    public virtual PurchaseOutOrder PurchaseOutOrder { get; set; }
    							
}