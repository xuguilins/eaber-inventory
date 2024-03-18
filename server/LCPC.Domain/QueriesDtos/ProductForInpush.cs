namespace LCPC.Domain.QueriesDtos;

public record ProductForInpush
{
    public string ProductName { get; set; }
    public string Id { get; set; }
    public string ProductCode { get; set; }
    public string ProductModel { get; set; }
    public int InventoryCount { get; set; }
    /// <summary>
    /// 商品进价
    /// </summary>
    public decimal Purchase { get; set; }
    /// <summary>
    /// 成本价
    /// </summary>
    public decimal InitialCost { get; set; }
    /// <summary>
    /// 批发价
    /// </summary>
    public decimal Wholesale { get; set; }
    /// <summary>
    /// 商品售价
    /// </summary>
    public decimal SellPrice { get; set; }
    public string CateName { get; set; }
    public string CateId { get; set; }
    public string UnitId { get; set; }
    public string UnitName { get; set; }
}