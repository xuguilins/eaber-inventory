namespace LCPC.Domain.QueriesDtos;

public record ProduceSellDto
{
    public string ProductCode { get; set; }
    public string ProductModel { get; set; }
    public string ProductName { get; set; }
    public string UnitName { get; set; }
    public int InventoryCount { get; set; }
    public decimal SellPrice { get; set; }
}

public record ProductCheck
{
    public string ProductCode { get; set; }
    public int Count { get; set; }
}