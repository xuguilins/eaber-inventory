namespace LCPC.Domain.Commands;

public class UpdatePuraseCommand:IRequest<ReturnResult>
{
    public string Id { get; set; }
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

    public List<PurashDetailOutDto> PrdocutDetail { get; set; }
}