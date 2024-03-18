namespace LCPC.Domain.Commands;

public class UpdateCustomerCommand:IRequest<ReturnResult>
{
    /// <summary>
    /// 主键
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 客户编码
    /// </summary>
    public string CustomerCode { get; set; }
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    public string CustomerUser { get; set; }
    /// <summary>
    /// 手机(非必填)
    /// </summary>
    public string TelNumber { get; set; }
    /// <summary>
    /// 电话(非必填)
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// 地址(非必填)
    /// </summary>
    public string Address { get; set; }

    public string Remark { get; set; }
    public bool Enable { get; set; }
}