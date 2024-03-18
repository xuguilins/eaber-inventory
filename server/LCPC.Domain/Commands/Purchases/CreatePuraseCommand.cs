namespace LCPC.Domain.Commands;

public class CreatePuraseCommand:IRequest<ReturnResult>
{
    /// <summary>
    /// 进货日期
    /// </summary>
    public string InOrderTime { get; set; }

    /// <summary>
    /// 供货渠道
    /// </summary>
    public ChannelType ChannelType { get; set; }

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
    /// 供应商主键
    /// </summary>
    public string SupplierId { get; set; }

    public string SupileName { get; set; }
    
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

    public List<PurashDetailDto> PrdocutDetail { get; set; }
}

public record PurashDetailOutDto : PurashDetailDto
{
    public string Id { get; set; }
    public int InvertCount { get; set; }
}
public record PurashDetailDto
{
   
    public string ProductCode { get; set; }
    public string ProductModel { get; set; }
    public string ProductName { get; set; }
    public int ProductCount { get; set; }
    public string Remark { get; set; }
    public string CateId { get; set; }
    public string UnitId { get; set; }
    /// <summary>
    /// 成本
    /// </summary>
    public decimal ProductIncost { get; set; }

    /// <summary>
    /// 批发价
    /// </summary>
    public decimal ProductWocost { get; set; }
    /// <summary>
    /// 进价
    /// </summary>
    public decimal ProductPrice { get; set; }

    /// <summary>
    /// 售价
    /// </summary>
    public decimal SellPrice { get; set; }
    public decimal ProductAll { get; set; }
}