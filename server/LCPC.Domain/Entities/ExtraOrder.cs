namespace LCPC.Domain.Entities;

public enum ExtraType
{
    ALL = -1 ,
    /// <summary>
    /// 支出
    /// </summary>
    Pay = 1,
    /// <summary>
    /// 收入
    /// </summary>
    InCOM = 2
}
/// <summary>
/// 额外订单
/// </summary>
public class ExtraOrder:EntityBase
{
    /// <summary>
    /// 编号
    /// </summary>
    public string OrderCode { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public ExtraType ExtraType { get; set; }

    /// <summary>
    /// 类型名称
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Price { get; set; }
}