namespace LCPC.Domain.EventHandlers.EventDatas;

public enum Option
{
    /// <summary>
    /// 释放库存
    /// product -count
    /// </summary>
    Cancale = 1,
    /// <summary>
    /// 增加库存 product+count
    /// </summary>
    Complete = 2
}
public class DisposeProduct:INotification
{
    public Option Option { get; set; }
    public int Count { get; set; }
    public string ProductId { get; set; }
}