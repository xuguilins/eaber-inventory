using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;

namespace LCPC.Hubs
{
    public class HubClient: Hub<IHubClient>
    { 
    
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
       
            Console.WriteLine("链接关闭");
            await Task.CompletedTask;
        }

        public override async Task OnConnectedAsync()
        {
            var a = Context.User.Claims.ToList();
            // HttpClient
            Console.WriteLine("用户已链接");
            await base.OnConnectedAsync();
            //await Task.CompletedTask;
        }
    }
}

