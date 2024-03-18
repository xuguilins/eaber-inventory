namespace LCPC.Domain.QueriesDtos;

public class OrderSearch : DataSearch
{
    public string Price { get; set; }
    public string UserName { get; set; }
    public string Tels { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
}

public class CusSearh : DataSearch
{
    public OrderStatus Status { get; set; }
}
public class UserOrderSearch : DataSearch
{
    public string UserId { get; set; }
    public OrderStatus Status { get; set; }
}
public class OrderInfoDto
{
    public string Id { get; set; }
    public string OrderCode { get; set; }
    public string OrderUser { get; set; }
    public string OrderTime { get; set; }
    public string OrderTel { get; set; }
    public decimal OrderPrice { get; set; }
    public string Remark { get; set; }
    public string PayName { get; set; }
    public string PayClient { get; set; }
    public OrderStatus  Status { get; set; }
}

public class SignleOrderInfo:OrderInfoDto
{
    public List<OrderDetailDto> DetailDtos { get; set; } = new List<OrderDetailDto>();
}

public class OrderDetailDto
{
    public int Index { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public int Count { get; set; }
    public string UnitName { get; set; }
    public decimal Price { get; set; }
    public string Remark { get; set; }
    public decimal AllPrice { get; set; }
    public string ParentId { get; set; }
}

public record OrderCountData
{
    public int AwaitCount { get; set; }
    public int CancleCount { get; set; }
    public int PayCount { get; set; }
    public int CompleteCount { get; set; }
}

public record OrderBuyUsers
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Tel { get; set; }
}

public record CustomerOrderDto
{
    [ExcelIgnore]
    public string UserId { get; set; }
    /// <summary>
    /// 客户名称
    /// </summary>
    [ExcelColumnName("客户名称")]
    public string CustomerName { get; set; }
    /// <summary>
    /// 客户联系人
    /// </summary>
    [ExcelColumnName("客户联系人")]
    public string CustomerUser { get; set; }
    
    /// <summary>
    /// 待支付
    /// </summary>
    [ExcelColumnName("待支付（元）")]
    public decimal dzf { get; set; }
    /// <summary>
    /// 已支付
    /// </summary>
     [ExcelColumnName("已支付（元）")]
    public decimal yzf { get; set; }
    /// <summary>
    /// 已完成
    /// </summary>
    [ExcelColumnName("已完成（元）")]
    public decimal ywc { get; set; }
    /// <summary>
    /// 作废
    /// </summary>
    [ExcelColumnName("已作废（元）")]
    public decimal zf { get; set; }
    /// <summary>
    /// 已取消
    /// </summary>
    [ExcelColumnName("已取消（元）")]
    public decimal yqx { get; set; }

}

public record CustomerOrderHeightDto
{
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string CustomerUser { get; set; }
    public string TelNumber { get; set; }
    public string OrderStatus { get; set; }
    public string OrderCode { get; set; }
     public string OrderTime { get; set; }
     public string OrderUser { get; set; }
     public string OrderTel { get; set; }
     public string OrderPay { get; set; }
     public decimal OrderMoney { get; set; }
     public string ProductCode { get; set; }
     public string ProductName { get; set; }
     public string UnitName { get; set; }
     public int OrderCount { get; set; }
     public decimal OrderSigle { get; set; }

     public decimal OrderPrice { get; set; }
    // a.CustomerCode, a.CustomerName,a.CustomerUser, a.TelNumber,b.OrderStatus,  b.OrderCode, b.OrderTime,  b.OrderUser, b.OrderTel,
    // b.OrderPay,b.OrderMoney, c.ProductCode,c.ProductName,c.UnitName,c.OrderCount,c.OrderSigle,c.OrderPrice 
}

public record HightDic
{
    public int Start { get; set; }
    public int End { get; set; }
}

public record ExportSearch
{
    /// <summary>
    /// 合并模式
    /// 1 自动合并
    /// 2 手动合并
    /// </summary>
    public int MaginType { get; set; }
    public OrderStatus OrderType { get; set; }
    public List<string> RangeTime { get; set; }
    public string UserName { get; set; }
}