using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.Commands;
using LCPC.Domain.QueriesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LCPC.Admin.Controllers
{
   [ApiExplorerSettings(GroupName ="供应商服务")]
   [Route("api/[controller]")]
   [ApiController]
   [Authorize]
   public class SupilerController : ControllerBase
   {
      private readonly IMediator _mediator;
      private readonly ISupilerQueries _supilerQueries;
      public SupilerController(IMediator mediator, ISupilerQueries supilerQueries)
      {
         _mediator = mediator;
         _supilerQueries = supilerQueries;
      }
      [HttpPost("createSupiler")]
      public async Task<ReturnResult> CreateSupiler(CreateProviderCommand command)
        => await _mediator.Send(command);

      [HttpPost("updateSupiler")]
      public async Task<ReturnResult> UpdateSupiler(UpdateProviderCommand command)
                => await _mediator.Send(command);
      [HttpDelete("deleteSupiler")]
      public async Task<ReturnResult> DeleteSupiler([FromBody]string[] ids)
      {
         var cmd = new DeleteProviderCommand();
         cmd.AddIds(ids);
         return await _mediator.Send(cmd);
      }

      [HttpPost("getSupilePage")]
      public async Task<ReturnResult<List<SupilerDto>>> GetSupilerPage(SupilerSearch search)
              => await _supilerQueries.GetSupilesPage(search);

      [HttpGet("getSupiler/{id}")]
      public async Task<ReturnResult<SupilerDto>> GetSignleSupiler(string id)
                     => await _supilerQueries.GetSignleSupiler(id);
      
      [HttpPost("updateSupilerStatus/{id}")]
      public async Task<ReturnResult> UpdateSupilerStatus(string id)  {
         var cmd = new UpdateProviderStatusCommand();
         cmd.AddId(id);
         return await _mediator.Send(cmd);
      }

      [HttpGet("getSupilers")]
      public async Task<ReturnResult<List<SupilerSelect>>> GetSupilers()
         => await _supilerQueries.GetSupilers();

      [HttpGet("getEnableSupilers")]
      public async Task<ReturnResult<List<EnableSupiler>>> GetEnableSupiles()
         => await _supilerQueries.GetEnableSupiles();

   }
}