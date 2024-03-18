namespace LCPC.Domain.QueriesDtos;

public class PuraseSearch:DataSearch
{
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string SupileName { get; set; }
    public string Tel { get; set; }
    public string UserName { get; set; }
}
public record PuraseInOrderDto
{
    public string Id { get; set; }
    public string InOrderCode { get; set; }
    public string InTime { get; set; }
    public ChannelType Chanpel { get; set; }
    public string Logistics { get; set; }
    public string InUser { get; set; }
    public string InPhone { get; set; }
    public string SupileName { get; set; }
    public string SupplierId { get; set; }
    public int AllCount { get; set; }
    public decimal AllPrice { get; set; }
    public InOStatus InOStatus { get; set; }
    public string Remark { get; set; }
}

public record PuraseOutDto
{
    public string Id { get; set; }
    public string InOrderCode { get; set; }
    /// <summary>
    /// 进货日期
    /// </summary>
    public string InOrderTime { get; set; }

    /// <summary>
    /// 供货渠道
    /// </summary>
    public int ChannelType { get; set; }
    
    /// <summary>
    /// 供应商主键
    /// </summary>
    public string SupplierId { get; set; }

    public string SupileName { get; set; }

    /// <summary>
    /// 物流单号
    /// </summary>
    public string Logistics { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string InUser { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string InPhone { get; set; }

    
    /// <summary>
    /// 进货总数
    /// </summary>
    public int InCount { get; set; }

    /// <summary>
    /// 进货总价
    /// </summary>
    public decimal InPrice { get; set; }

    /// <summary>
    /// 进货单状态
    /// </summary>
    public InOStatus InOStatus { get; set; }
    

    public string Remark { get; set; }

    public List<PurashDetailOutDto> PrdocutDetail { get; set; }
}

public record PurasheOutModalDto
{
    public string Id { get; set; }
    public string SupilerId { get; set; }
    public string UserName { get; set; }
    public string UserTel { get; set; }
    public string Code { get; set; }
    public string PushTime { get; set; }
}

public record PurasheOutModalDetail
{
    public string Id { get; set; }
    public string ProductCode { get; set; }
    public string ProductName { get; set; }

    public string ProductModel { get; set; }

    public int ProductCount { get; set; }

    public decimal InPrice { get; set; }

    public int ReturnCount { get; set; }
    public decimal OutPrice { get; set; }
    public decimal OutAll { get; set; }

    //序号	操作	产品名称	型号	进货数量	进货价	退货数量	退货价格	退货总价
}

public record PuraseOutOrderDto
{
    public string Id { get; set; }
    public string OutOrderCode { get; set; }
    public string OutTime { get; set; }
    public string Logistics { get; set; }
    public string OutUser { get; set; }
    public string OutPhone { get; set; }
    public string SupileName { get; set; }
    public string SupplierId { get; set; }
    public int OutAllCount { get; set; }
    public decimal OutAllPrice { get; set; }
    public OutStatus  OutStatus { get; set; }
    public string Remark { get; set; }
}