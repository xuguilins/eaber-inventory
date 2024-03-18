

namespace LCPC.Domain.QueriesDtos;

public class ProductExcelDto
{
    /// <summary>
    /// 商品名称
    /// </summary>
    [ExcelColumnName("商品名称")]
    public string ProductName { get; set; }
    /// <summary>
    ///  品名规格(必填)	
    /// </summary>
    [ExcelColumnName("商品规格")]
    public string ProductModel { get; set; }
    /// <summary>
    /// 货品类别
    /// </summary>
    [ExcelColumnName("商品类别")]
    public string  Cate { get; set; }
    /// <summary>
    /// 主计量单位
    /// </summary>
    [ExcelColumnName("单位")]
    public  string Unit { get; set; }
    /// <summary>
    /// 供应商名称
    /// </summary>

    [ExcelColumnName("供应商")]
    public string SupileName { get; set; }
    /// <summary>
    /// 换算率（辅/主）
    /// </summary>
    [ExcelColumnName("换算率")]
    public string ConversionRate { get; set; }
    /// <summary>
    /// 库存数量
    /// </summary>
    [ExcelColumnName("库存数量")]
    public string InventoryCount { get; set; }
    /// <summary>
    /// 期初成本
    /// </summary>
    [ExcelColumnName("期初成本")]
    public decimal InitialCost { get; set; }
    /// <summary>
    /// 进货价
    /// </summary>
    [ExcelColumnName("进货价")]
    public decimal Purchase { get; set; }
    /// <summary>
    /// 零售价
    /// </summary>
    [ExcelColumnName("零售价")]
    public decimal SellPrice { get; set; }
    /// <summary>
    /// 批发价
    /// </summary>
    [ExcelColumnName("批发价")]
    public decimal Wholesale { get; set; }
    /// <summary>
    /// 最高库存
    /// </summary>
    [ExcelColumnName("最高库存")]
    public string MaxStock { get; set; }
    /// <summary>
    /// 最低库存
    /// </summary>
    [ExcelColumnName("最高库存")]
    public string MinStock { get; set; }
    [ExcelColumnName("备注")]
    public string Remark { get; set; }
}