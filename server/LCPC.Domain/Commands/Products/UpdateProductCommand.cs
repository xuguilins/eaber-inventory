namespace LCPC.Domain.Commands;



public class UpdateProductCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }

    public string ProductCode { get; set; }
    /// <summary>
    /// 商品名称
    /// </summary>
 
    public string ProductName { get;  set; }
    /// <summary>
    ///  品名规格(必填)	
    /// </summary>
   
    public string ProductModel { get;  set; }
    /// <summary>
    /// 货品类别
    /// </summary>
 
    public string  CateId { get;  set; }
    /// <summary>
    /// 主计量单位
    /// </summary>
 
    public  string UnitId { get;  set; }
    
    public string SupilerId { get; set; }
    /// <summary>
    /// 换算率（辅/主）
    /// </summary>
 
    public string ConversionRate { get;  set; }
    /// <summary>
    /// 库存数量
    /// </summary>

    public int InventoryCount { get; set; }
    /// <summary>
    /// 期初成本
    /// </summary>

    public decimal InitialCost { get;  set; }
    /// <summary>
    /// 进货价
    /// </summary>
  
    public decimal Purchase { get;  set; }
    /// <summary>
    /// 零售价
    /// </summary>
    
    public decimal SellPrice { get;  set; }
    /// <summary>
    /// 批发价
    /// </summary>
   
    public decimal Wholesale { get;  set; }
    /// <summary>
    /// 最高库存
    /// </summary>

    public int MaxStock { get;  set; }

    public int MinStock { get;  set; }

    public string Remark { get;  set; }
    public bool Enable { get; set; }
}