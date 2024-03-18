namespace LCPC.Domain.QueriesDtos;

public record HomeCardDto
{
    /// <summary>
    /// 商品总数--日总数
    /// </summary>
    public string ProductCount  { get; set; }

    /// <summary>
    /// 销售总额-日销售额
    /// </summary>
    public string SellPrice { get; set; }

    /// <summary>
    /// 利润总额-日利润
    /// </summary>
    public string ProfilePrice { get; set; }

    /// <summary>
    /// 订单总是--日订单
    /// </summary>
    public string OrderCount { get; set; }
}

public record SellProfileDto
{
    /// <summary>
    /// 销售价
    /// </summary>
    public decimal OrderPrice { get; set; }
    /// <summary>
    ///  成本价格 = 批发价+成本
    /// </summary>
    public decimal OurMoney { get; set; }
    /// <summary>
    /// 利润
    /// </summary>
    public decimal OurProfile { get; set; }

    public DateTime OrderTime { get; set; }
}

public enum FilterType
{
    Year = 1,
    Month = 2,
    Week =3,
    Rang = 4
}


public record ColumnCardDto
{
    public string[] XTypes { get; set; }
    public double[] YTypes { get; set; }
}

public record OrderCardDto
{
    /// <summary>
    /// xz轴时间
    /// </summary>
    public string[] XTypes { get; set; }

    public int[] DZFTypes { get; set; }
    public int[] YZFTypes { get; set; }
    public int[] YWCTypes { get; set; }
    public int[] ZFTypes { get; set; }
    public int[] YQXTypes { get; set; }
}

public record HeightProduct
{
    public int SellCount { get; set; }
    public string ProductName { get; set; }
}
public enum ColumnType
{
    /// <summary>
    /// 销售额
    /// </summary>
    SellType = 1,
    /// <summary>
    /// 进货情况
    /// </summary>
    InProduct =2,
    /// <summary>
    /// 退货情况
    /// </summary>
    OutProduct =3,
    /// <summary>
    /// 订单量
    /// </summary>
    Order = 4
}

public record ColumnsDto
{
    public ColumnType ColumnType { get; set; }

    public FilterType FilterType { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
}

public record ColumnsQueryDto
{
    public double Value { get; set; }
    public string Label { get; set; }
}

public record OrderQueryDto
{
    public int NOWMONTH { get; set; }
    public int NOWCOUNT { get; set; }
    public int NOWSTATUS { get; set; }
    public int NOWDAY { get; set; }
}

public record OrderQueryWeekDto
{

    public int NOWCOUNT { get; set; }
    public int OrderStatus { get; set; }
    public string OrderTime { get; set; }
}

public record SystemInfoDto
{
    public string ProductName { get; set; }
    public string ProductVersion { get; set; }
    public string ProductType { get; set; }
    public string AuthTarget { get; set; }
    public string BackService { get; set; }
    public string FrontService { get; set; }
    public string Deployment { get; set; }
    public string DatabaseType { get; set; }
    public string DatabaseVersion { get; set; }

    public string HostName { get; set; }
    public string SystemName { get; set; }
    public string MacAddress { get; set; }
    public string OffSetDay { get; set; }
    public string EndTime { get; set; }
}