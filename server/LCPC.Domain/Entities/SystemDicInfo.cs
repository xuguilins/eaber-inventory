using System.ComponentModel;

namespace LCPC.Domain.Entities;

/// <summary>
/// 系统字典维护
/// </summary>
public enum DicType
{
    /// <summary>
    /// 用于查询
    /// </summary>
    ALL = -1,
    [Description("单位管理")]
    UNIT= 1,
    [Description("支付方式")]
    PAY = 2,
    
    [Description("其它收入")]
    OthersIN  = 3,
    [Description("其它支出")]
    OthersOut = 4
}
public class SystemDicInfo:EntityBase
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public DicType  DicType { get; set; }
    /// <summary>
    /// 字典名称
    /// </summary>
    public string DicName { get; set; }

    /// <summary>
    /// 字典代码/值
    /// </summary>
    public string DicCode { get; set; }
}