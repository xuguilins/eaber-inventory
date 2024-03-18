using LCPC.Domain.Commands;
using LCPC.Domain.QueriesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LCPC.Admin.Controllers;

[ApiExplorerSettings(GroupName ="进货退货服务")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PuraseOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPuraseInOrderQueries _puraseInOrderQueries;
    public PuraseOrderController(IMediator mediator,IPuraseInOrderQueries puraseInOrderQueries)
    {
        _mediator = mediator;

        _puraseInOrderQueries = puraseInOrderQueries;
    }


    #region 进货单

    

    [HttpPost("createPushOrder")]
    public async Task<ReturnResult> CreatePurasheInOrder([FromBody] CreatePuraseCommand command)
        => await _mediator.Send(command);

    [HttpPost("getInPurasePage")]
    public async Task<ReturnResult< List<PuraseInOrderDto>>> GetInPurasePage(PuraseSearch search)
        => await _puraseInOrderQueries.GetInPurasePage(search);

    [HttpGet("getPurase/{id}")]
    public async Task<ReturnResult<PuraseOutDto>> GetPuraseInfo(string id)
        => await _puraseInOrderQueries.GetPuraseInfo(id);

    [HttpPost("updatePushOrder")]
    public async Task<ReturnResult> UpdatePurasheInOrder([FromBody]UpdatePuraseCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deletePurashe")]
    public async Task<ReturnResult> DeletePurasheOrders([FromBody]string[] ids)
    {
        DeletePuraseCommand cmd = new DeletePuraseCommand();
        cmd.AddIds(ids);
        return await _mediator.Send(cmd);
    }

    [HttpPost("updatePurashStatus")]
    public async Task<ReturnResult> UpdatePurasheStatus([FromBody] UpdatePuraseStatusCommand command)
        => await _mediator.Send(command);

    [HttpPost("getPurashModal")]
    public async Task<ReturnResult<List<PurasheOutModalDto>>> GetPurashModalPage([FromBody]DataSearch search)
        => await _puraseInOrderQueries.GetPurashModalPage(search);

    [HttpGet("getModalDetail/{id}")]
    public async Task<ReturnResult<List<PurasheOutModalDetail>>> GetPurashModalDetail(string id)
        => await _puraseInOrderQueries.GetPurashModalDetail(id);

    [HttpPost("getPurashInCusPage")]
    public async Task<ReturnResult<List<PurashInOrderDtoRecord>>> GetSupileCusPage([FromBody]DataSearch search)
        => await _puraseInOrderQueries.GetSupileCusPage(search);

    [HttpPost("exportCusExcel")]
    public async Task<FileResult> ExportCusExcel()
    {
        var data = await _puraseInOrderQueries.ExportCusExcel();
        return new FileContentResult(data, "application/vnd.ms-excel");
    }

    [HttpPost("exportCusHeightExcel")]
    public async Task<FileResult> ExportCusHegithExcel([FromBody]PurashExcelSearch search)
    {
        var data = await _puraseInOrderQueries.ExportCusHegithExcel(search);
        return new FileContentResult(data, "application/vnd.ms-excel");
    }

    [HttpPost("getPurashInCusTablePage")]
    public async Task<ReturnResult<List<PurashInOrderTabDetail>>> GetSupileCusTable(SupPuSearch search)
        => await _puraseInOrderQueries.GetSupileCusTable(search);

    [HttpGet("getPurashInCusDetail/{Id}")]
    public async Task<ReturnResult<List<PurashInOrderDetailRecord>>> GetSupileCusDetail(string Id)
        => await
            _puraseInOrderQueries.GetSupileCusDetail(Id);
    
    #endregion

    #region 退货单

    [HttpPost("createOutPurashe")]
    public async Task<ReturnResult> CreateOutPurase(PurashOutCommand command)
        => await _mediator.Send(command);

    [HttpPost("getOutPurasePage")]
    public async Task<ReturnResult<List<PuraseOutOrderDto>>> GetOutPurasePage([FromBody]PuraseSearch search)
        => await _puraseInOrderQueries.GetOutPurasePage(search);

    [HttpGet("getOutOrder/{id}")]
    public async Task<ReturnResult<PuraseOutOrderSigleDto>> GetSigleOutPurashInfo(string id)
        => await _puraseInOrderQueries.GetSigleOutPurashInfo(id);

    [HttpPost("updateOutPurashe")]
    public async Task<ReturnResult> UpdateOutPurase([FromBody] UpdatePurashOutCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deleteOutPurase")]
    public async Task<ReturnResult> DeleteOutPurase([FromBody] string[] ids)
    {
        var com = new DeletePurashOutCommand();
        com.AddIds(ids);
        return await _mediator.Send(com);
    }

    [HttpPost("updateOutPurashStatus")]
    public async Task<ReturnResult> UpdateOutPuraseStatus([FromBody] UpdatePurashOutStatusCommand command)
        => await _mediator.Send(command);

    #endregion
}