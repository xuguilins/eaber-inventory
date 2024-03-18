using MiniExcelLibs;

namespace LCPC.Domain.Entities;

public class PurchaseInDetail:EntityBase
{
    public string PurchaseInId { get; set; }
    public virtual PurchaseInOrder PurchaseInOrder { get; set; }
    public string ProductId { get; set; }
    public string ProductCode { get; set; }
    public string ProductModel { get; set; }
    public string ProductName { get; set; }
    /// <summary>
    /// 进货数量
    /// </summary>
    public int ProductCount { get; set; }
    /// <summary>
    /// 进货价
    /// </summary>
    public decimal ProductPrice { get; set; }
    
    /// <summary>
    /// 进货总价
    /// </summary>
    public decimal ProductAll { get; set; }
}