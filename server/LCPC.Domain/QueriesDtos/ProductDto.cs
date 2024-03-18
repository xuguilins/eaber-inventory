namespace LCPC.Domain.QueriesDtos;

public class ProductSearch : DataSearch
{
    public string ProductModel { get; set; }
    public string Remark { get; set; }
    public string CateId { get; set; }
    public string SupileName { get; set; }
 
}

public record ProductCate
{
    public string Id { get; set; }
    public string CateName { get; set; }
    public string ParentId { get; set; }
    public int COUNTVALUE { get; set; }
   
}
public record ProductCatesDto
{
    public string CateId { get; set; }
    public string CateName { get; set; }
    public int Count { get; set; }
    
    public List<ProductCatesDto> Children { get; set; }
}
public record ProductDto
{
    public string Id { get; private set; }
    public string ProductCode { get; set; }
    /// <summary>
    /// 商品名称
    /// </summary>
 
    public string ProductName { get; private set; }
    /// <summary>
    ///  品名规格(必填)	
    /// </summary>
   
    public string ProductModel { get; private set; }
    /// <summary>
    /// 货品类别
    /// </summary>
 
    public string  CateName { get; private set; }

    public string UnitId { get; set; }
    public string SupilerId { get; set; }
    public string CateId { get; set; }
    /// <summary>
    /// 主计量单位
    /// </summary>
 
    public  string UnitName { get;private  set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string SupName { get; set; }
    /// <summary>
    /// 换算率（辅/主）
    /// </summary>
 
    public string ConversionRate { get;private  set; }
    /// <summary>
    /// 库存数量
    /// </summary>

    public string InventoryCount { get;private set; }
    /// <summary>
    /// 期初成本
    /// </summary>

    public decimal InitialCost { get;private  set; }
    /// <summary>
    /// 进货价
    /// </summary>
  
    public decimal Purchase { get; private set; }
    /// <summary>
    /// 零售价
    /// </summary>
    
    public decimal SellPrice { get;private  set; }
    /// <summary>
    /// 批发价
    /// </summary>
   
    public decimal Wholesale { get;private  set; }
    /// <summary>
    /// 最高库存
    /// </summary>

    public string MaxStock { get;private  set; }

    public string MinStock { get;private  set; }

    public string Remark { get; private set; }
    public bool Enable { get; set; }
}

public record PushProdcutDto
{
    public string Produt { get; set; }
}