using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LCPC.Admin.Controllers;

[ApiExplorerSettings(GroupName ="即时通讯")]
[Route("api/[controller]")]
[ApiController]
public class HubCenterController : Controller
{
    private readonly IHubCenterService _hubCenterService;
    public HubCenterController(IHubCenterService hubCenterService)
    {
        _hubCenterService = hubCenterService;
    }
    [HttpGet("sendMessage")]
    public async Task<ReturnResult> SendMessage(string userId,string message)
    {
        return await _hubCenterService.PushMessageForUser(userId,message);
    }
}