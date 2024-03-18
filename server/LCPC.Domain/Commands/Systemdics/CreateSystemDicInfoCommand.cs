namespace LCPC.Domain.Commands;

public class CreateSystemDicInfoCommand:IRequest<ReturnResult>
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

    public string Remark { get; set; }
    public bool Enable { get; set; }
}