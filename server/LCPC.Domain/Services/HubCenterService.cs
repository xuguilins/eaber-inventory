using LCPC.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LCPC.Domain.Services;

public class HubCenterService:IHubCenterService
{
    private readonly IHubContext<HubClient, IHubClient> _hubContext;
    private readonly IUserIdProvider _userIdProvider;
    public HubCenterService(IHubContext<HubClient, IHubClient> hubContext,IUserIdProvider userIdProvider)
    {
        _hubContext = hubContext;
        _userIdProvider = userIdProvider;
    }

    public async Task<ReturnResult> PushMessageForUser(string userName,string message)
    {
     
        await _hubContext.Clients.User(userName).SendUser(message);
        return new ReturnResult(true, null, "发送成功");
    }
}