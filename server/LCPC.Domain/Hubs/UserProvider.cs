using Microsoft.AspNetCore.SignalR;

namespace LCPC.Domain.Hubs;

public class UserProvider:IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        string user = string.Empty;
        var cliams = connection.User.Claims.ToList();
        var cliam = cliams.FirstOrDefault(d => d.Type == "userName");
        if (cliam != null)
            user = cliam.Value;
        return user;
    }
}