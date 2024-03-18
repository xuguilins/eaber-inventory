namespace LCPC.Domain.Entities;

public class ProductInfo :EntityBase
{
    /// <summary>
    /// 货品代码
    /// </summary>
    public string ProductCode { get; set; }
    /// <summary>
    /// 商品名称	
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 商品简称
    /// </summary>
    public string NameSpell { get; set; }
    /// <summary>
    ///  品名规格(必填)	
    /// </summary>
    public string ProductModel { get; set; }
    /// <summary>
    /// 货品类别
    /// </summary>
    public string CateId { get; set; }
    public virtual CateInfo Cate { get; set; }
    public string UnitId { get; set; }


    public string SupilerId { get; set; }
    public virtual SupplierInfo Supplier { get; set; }
    /// <summary>
    /// 换算率（辅/主）
    /// </summary>
    public string ConversionRate { get; set; }
    /// <summary>
    /// 库存数量
    /// </summary>
    public int InventoryCount { get; set; }
    /// <summary>
    /// 期初成本
    /// </summary>
    public decimal InitialCost { get; set; }
    /// <summary>
    /// 进货价
    /// </summary>
    public decimal Purchase { get; set; }
    /// <summary>
    /// 零售价
    /// </summary>
    public decimal SellPrice { get; set; }
    /// <summary>
    /// 批发价
    /// </summary>
    public decimal Wholesale { get; set; }
    /// <summary>
    /// 最高库存
    /// </summary>
    public int MaxStock { get; set; }
    /// <summary>
    /// 最低库存
    /// </summary>
    public int MinStock { get; set; }
}