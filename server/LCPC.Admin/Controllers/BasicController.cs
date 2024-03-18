using System.Formats.Asn1;
using LCPC.Domain.Commands;
using LCPC.Domain.Entities;
using LCPC.Domain.QueriesDtos;
using LCPC.Share.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace LCPC.Admin.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[ApiExplorerSettings(GroupName = "基础服务")]
[Route("api/[Controller]")]
[ApiController]
[Authorize]
public class BasicController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICateInfoQueries _cateInfoQueries;
    private readonly IRuleInfoQueries _ruleQueries;
    
    private readonly ICustomerQueries _customerQueries;
    private readonly ISystemDicQueries _systemDicQueries;

    public BasicController(IMediator mediator,
        ICateInfoQueries cateInfoQueries,
        IRuleInfoQueries ruleInfoQueries,
       
        ICustomerQueries cyCustomerQueries,
        ISystemDicQueries systemDicQueries
        
    )
    {
        _mediator = mediator;
        _cateInfoQueries = cateInfoQueries;
        _ruleQueries = ruleInfoQueries;
  
        _customerQueries = cyCustomerQueries;
        _systemDicQueries = systemDicQueries;
    }

    #region 分类管理

    [HttpPost("createCate")]
    public async Task<ReturnResult> CreateCate(CreateCateCommand command)
        => await _mediator.Send(command);

    [HttpPost("updateCate")]
    public async Task<ReturnResult> UpdateCate(UpdateCateCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deleteCate")]
    public async Task<ReturnResult> DeleteCate([FromBody] string[] ids)
    {
        var cmd = new DeleteCateCommand();
        cmd.AddIds(ids);
        return await _mediator.Send(cmd);
    }

    [HttpPost("getCatePages")]
    public async Task<ReturnResult<List<CateInfoDto>>> GetRevolveCates(CateSearch search)
        => await _cateInfoQueries.GetPageCates(search);

    [HttpPost("updateCateStatus/{id}")]
    public async Task<ReturnResult> UpdateCateStatus(string id)
    {
        var command = new UpdateCateStatusCommand();
        command.AddId(id);
        return await _mediator.Send(command);
    }
    [HttpGet("getTreeCates")]
    public async Task<ReturnResult<List<CateTreeDto>>> GetTreeCates(bool enable)
        => await _cateInfoQueries.GetTreeCates(enable);

    [HttpGet("getCate/{id}")]
    public async Task<ReturnResult<CateInfoDto>> GetCateIInfo(string id)
        => await _cateInfoQueries.GetCateInfo(id);

    #endregion

    #region 规则管理

    [HttpPost("createRule")]
    public async Task<ReturnResult> CreatRule(CreateRuleCommand command)
        => await _mediator.Send(command);

    [HttpPost("updateRule")]
    public async Task<ReturnResult> UpdateRule(UpdateRuleCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deleteRule")]
    public async Task<ReturnResult> DeleteRule([FromBody] string[] ids)
    {
        var cmd = new DeleteRuleCommand();
        cmd.AddIds(ids);
        return await _mediator.Send(cmd);
    }

    [HttpPost("updateRuleStatus/{id}")]
    public async Task<ReturnResult> UpdateRuleStatus(string id)
    {
        var cmd = new UpdateRuleStatusCommand();
        cmd.AddId(id);
        return await _mediator.Send(cmd);
    }

    [HttpPost("getRulesPage")]
    public async Task<ReturnResult<List<RuleInfoDto>>> GetRulesPage(DataSearch search)
        => await _ruleQueries.GetRulePages(search);

    [HttpGet("getRuleInfo/{id}")]
    public async Task<ReturnResult<RuleInfoDto>> GetSignleRule(string id)
        => await _ruleQueries.GetSignleRule(id);

    #endregion

    #region 支付方式
    
    [HttpGet("getPays")]
    public async Task<ReturnResult<List<EnablePayInfoDto>>> GetPays()
        => await _systemDicQueries.GetPays();

    #endregion

    #region 客户管理
    [HttpPost("createCustomer")]

    public async Task<ReturnResult> CreateCustomer([FromBody]CreateCustomerCommand command)
        => await _mediator.Send(command);
    [HttpPost("updateCustomer")]
    public async Task<ReturnResult> UpdateCustomer([FromBody]UpdateCustomerCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deleteCustomer")]

    public async Task<ReturnResult> DeleteCustomer([FromBody]string[] ids)
    {
        DeleteCustomerCommand comdCommand = new DeleteCustomerCommand();
        comdCommand.AddIds(ids);
        return await _mediator.Send(comdCommand);
    }

    [HttpPost("updateCustomerStatus/{id}")]
    public async Task<ReturnResult> UpdateCusStatus(string id)
    {
        var com = new UpdateCustomerStatusCommand();
        com.AddId(id);
        return await _mediator.Send(com);
    }

    [HttpPost("getCustomerPage")]
    public async Task<ReturnResult<List<CustomerDto>>> GetCustomerPages([FromBody]DataSearch search)
        => await _customerQueries.GetCustomerPages(search);

    #endregion

    #region 系统字典

    [HttpPost("createSystemDic")]
    public async Task<ReturnResult> CreateSystemDic(CreateSystemDicInfoCommand command)
        => await _mediator.Send(command);
    [HttpPost("updateSystemDic")]
    public async Task<ReturnResult> UpdateSystemDic(UpdateSystemDicInfoCommand command)
        => await _mediator.Send(command);

    [HttpPost("updateSystemStatus/{id}")]
    public async Task<ReturnResult> UpdateSystemDicStatus(string id)
    {
        var com = new UpdateSystemDicInfoStatusCommand();
        com.AddId(id);
        return await _mediator.Send(com);
    }

    [HttpDelete("deleteSystem")]
    public async Task<ReturnResult> DeleteSystemDic([FromBody]string[] ids)
    {
        var com = new DeleteSystemDicInfoCommand();
        com.AddIds(ids);
        return await _mediator.Send(com);
    }

    [HttpPost("getSystemDicPage")]
    [AllowAnonymous]
    public async Task<ReturnResult<List<SystemDicDto>>> GetSystemDicPage(DicSearch search)
        => await _systemDicQueries.GetSystemDicPage(search);
    
    [HttpGet("getDicTypes")]
    public async Task<ReturnResult<List<DicModel>>> GetDicTypes()
        => await _systemDicQueries.GetDicTypes();
    [HttpGet("getRuleTypes")]
    public async Task<ReturnResult<List<DicModel>>> GetRuleTypes()
        => await _systemDicQueries.GetRuleTypes();

    [HttpGet("getUnits")]
    public async Task<ReturnResult<List<UnitInfoDto>>> GetUnits()
        => await _systemDicQueries.GetUnits();
    [HttpGet("getSysDics")]
    public async Task<ReturnResult<List<SysDicData>>> GetSysDics(DicType type)
        => await _systemDicQueries.GetDics(type);
    [HttpGet("getst")]
    [AllowAnonymous]
    public async Task Test()
        => await _systemDicQueries.test();

    #endregion


}