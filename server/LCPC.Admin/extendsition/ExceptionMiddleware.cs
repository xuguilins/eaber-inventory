using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Share;

namespace LCPC.Admin.extendsition
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next(context);
            }catch (Exception ex){
                LoggerManager.LoggerError($"异常信息：{ex.Message} \r\n ,详细信息:{ex.InnerException?.Message} \r\n" +
                                          $"{ex.StackTrace }",ex);
                var result = new ReturnResult(false,null,ex.Message);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
               await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}