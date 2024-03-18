using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LCPC.Admin.Controllers
{
    [ApiExplorerSettings(GroupName ="数据导入")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExcelController(IMediator mediator, ISupilerQueries supilerQueries)
        {
            _mediator = mediator;

        }
        [HttpPost("importExcel")]
        public async Task<ReturnResult> ExportSupiler(IFormFile file, int type)
        {
            if (file == null)
                throw new Exception("无效的文件");
            var ext = Path.GetExtension(file.FileName);
            string[] exts = new string[] { ".xlsx", ".xls", ".csv" };
            if (!exts.Contains(ext))
                throw new Exception($"仅支持[{string.Join(",", exts)}]格式");
            var path = AppDomain.CurrentDomain.BaseDirectory + "/a.xlsx";
            var steam = file.OpenReadStream();
            var bytes = new byte[steam.Length];
            steam.Read(bytes, 0, bytes.Length);
            steam.Close();
            var cmd = CommandBuilder.CreaeCommand(type, bytes);
            var result = await _mediator.Send(cmd) as ReturnResult;
            return result;
        }
    }
}