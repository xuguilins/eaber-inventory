namespace LCPC.Domain;

public static class MessageHelper
{
    public static string DeleteMessage(int count, bool success = true)
    =>success ? $"成功删除【{count}】条数据" : $"【{count}】条数据删除失败";
}