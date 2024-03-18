namespace LCPC.Domain.Entities;

public class RuleInfo:EntityBase
{
    /// <summary>
    /// 规则类型
    /// </summary>
    public RuleType RuleType { get; set; }
    /// <summary>
    /// 规则名称
    /// </summary>
    public string RuleName { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    public string RulePix { get; set; }

    /// <summary>
    /// 格式
    /// </summary>
    public string Formatter { get; set; }

    /// <summary>
    /// 自增数
    /// </summary>
    public int IdentityNum { get; set; }

    /// <summary>
    /// 规则补位数
    /// </summary>
    public int RuleAppend { get; set; }

    /// <summary>
    /// 当前值
    /// </summary>
    public int NowValue { get; set; }
}