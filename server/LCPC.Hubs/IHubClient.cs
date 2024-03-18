namespace LCPC.Hubs;
public interface IHubClient
{
 
    Task SendAll(object data);


    /// <summary>
    /// 给指定的链接id发送消息
    /// </summary>
    /// <param name="connectionId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendConnection(string connectionId, string message);

    /// <summary>
    /// 给特定的用户发送消息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendUser(string message);
}