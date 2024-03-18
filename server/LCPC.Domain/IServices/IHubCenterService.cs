namespace LCPC.Domain.IServices;

public interface IHubCenterService:IScopeDependecy
{
    Task<ReturnResult> PushMessageForUser(string userName,string message);
}