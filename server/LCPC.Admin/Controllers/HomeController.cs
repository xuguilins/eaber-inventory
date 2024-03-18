using LCPC.Domain.Entities;
using LCPC.Domain.QueriesDtos;
using LCPC.Infrastructure.Cores;
using LCPC.Share;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LCPC.Admin.Controllers;

[ApiExplorerSettings(GroupName ="首页")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly IRuleManager _ruleManager;

    private readonly AdminDbContext _adminDbContext;
    public HomeController(IHomeService homeService,
        ILogger<HomeController> logger,
        AdminDbContext adminDbContext,
        IRuleManager ruleManager)
    {

        _homeService = homeService;
        _ruleManager = ruleManager;
        _adminDbContext = adminDbContext;
    }
    [AllowAnonymous]

    [HttpGet("getHomeCard")]

    public async Task<ReturnResult<HomeCardDto>> GetHomeStatic()
    {
        return await _homeService.GetHomeStatic();
    }


    [HttpPost("getColumns")]
    public async Task<ReturnResult<ColumnCardDto>> GetColumns([FromBody]ColumnsDto dto)
        => await _homeService.GetColumns(dto);
    [HttpGet("getSystemInfo")]
    public async Task<ReturnResult<SystemInfoDto>> GetSystem()
        => await _homeService.GetSystem();
    
     [HttpPost("getOrderColumns")]
    public async Task<ReturnResult<OrderCardDto>> GetOrderColumns([FromBody]ColumnsDto dto)
        => await _homeService.GetOrderColumns(dto);

    [HttpPost("getHeightProduct")]
    public async Task<ReturnResult<List<HeightProduct>>> GetHeightProduct([FromBody]ColumnsDto dto)
        => await _homeService.GetHeightProduct(dto);

    [AllowAnonymous]
    [HttpGet("updateRows")]
    public async Task<ReturnResult> UpdateProdcutName()
        => await _homeService.UpdateProdcutName();



}