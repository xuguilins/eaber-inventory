namespace LCPC.Domain.QueriesDtos;

public record PuraseOutOrderSigleDto
{
    public string Id { get; set; }
    public string OutOrderCode { get; set; }
    public string InOrderCode { get; set; }
    public string Reason { get; set; }
    public string InPhone { get; set; }
    public string OutOrderTime { get; set; }
    public string InUser { get; set; }
    public string Logics { get; set; }
    public string SupilerId { get; set; }
    public string Remark { get; set; }
    public string SupildName { get; set; }
    public int OutStatus { get; set; }
    public List<PuraseOutOrderSigleDetail> Details { get; set; } = new List<PuraseOutOrderSigleDetail>();
}

public record PuraseOutOrderSigleDetail
{
    public string Id { get; set; }
    public decimal InPrice { get; set; }
    public decimal OutPrice { get; set; }
    public string ProductCode { get; set; }
    public int ProductCount { get; set; }
    public int ReturnCount { get; set; }
    public string ProductModel { get; set; }
    public string ProductName { get; set; }
}

public record PurashInOrderDtoRecord
{
    [ExcelColumn(Name = "供应商名称")]
    public string SupName { get; set; }
    [ExcelColumn(Name = "供应商联系人")]
    public string ProviderUser { get; set; }
    [ExcelColumn(Name = "联系方式")]
    public string SupPhone { get; set; }
    [ExcelColumn(Name = "进行中金额")]
    public decimal JXZ { get; set; }
    [ExcelColumn(Name = "已完成金额")]
    public decimal YWC { get; set; }
    [ExcelColumn(Ignore = true)]
    public string SupId { get; set; }
}

public record PurashInOrderTabDetail
{
    public string Id { get; set; }
    public string PurchaseCode { get; set; }
    public string InOrderTime { get; set; }
    public string ProductCode { get; set; }
    public ChannelType ChannelType { get; set; }
    public string SupName { get; set; }
    public string InUser { get; set; }
    public string InPhone { get; set; }
    public int InCount { get; set; }
    public decimal InPrice { get; set; }
}
public record PurashInOrderDetailRecord
{
    public string Id { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductModel { get; set; }
    public int ProductCount { get; set; }
    public decimal ProductPrice { get; set; }
    public decimal ProductAll { get; set; }
}

public class SupPuSearch : DataSearch
{
    public int Status { get; set; }
    public string SupId { get; set; }
}

public class PurashExcelSearch
{
    /// <summary>
    /// 合并模式
    /// 1 自动合并
    /// 2 手动合并
    /// </summary>
    public int MaginType { get; set; }
    /// <summary>
    /// 进货单状态 
    /// </summary>
    public InOStatus InorOStatus { get; set; }
    /// <summary>
    /// 单据时间
    /// </summary>
    public List<string> RangeTime { get; set; }
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string UserName { get; set; }
}

public record PurashInExcelDetail
{
    public string SupNumber { get; set; }
    public string SupName { get; set; }
    public string ProviderUser { get; set; }
    public string InOStatus { get; set; }
    public string InOrderTime { get; set; }
    public string PurchaseCode { get; set; }
    public int InCount { get; set; }
    public string InPrice { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public string ProductModel { get; set; }
    public int ProductCount { get; set; }
    public string ProductPrice { get; set; }
    public string ProductAll { get; set; }
}