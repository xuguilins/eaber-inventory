using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.Commands;
using LCPC.Domain.Entities;
using LCPC.Domain.QueriesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LCPC.Admin.Controllers
{
    [ApiExplorerSettings(GroupName ="订单服务")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;
        private readonly IExtraOrderQueries _extraOrderQueries;

        public OrderController(IMediator mediator,IOrderQueries orderQueries,IExtraOrderQueries extraOrderQueries)
        {
            _mediator = mediator;
            _extraOrderQueries = extraOrderQueries;
            _orderQueries = orderQueries;
        }

        [HttpPost("createOrder")]
        public async Task<ReturnResult> CreateOrder(CreateOrderCommand command)
                  => await _mediator.Send(command);

        [HttpPost("getOrderPage")]
        public async Task<ReturnResult<List<OrderInfoDto>>> GetOrderPage([FromBody]OrderSearch search,int type)
            => await _orderQueries.GetOrderPage(search, type);

        [HttpGet("getOrderCount")]
        public async Task<ReturnResult<long>> GetOrderCount(OrderStatus status = OrderStatus.AWAIT)
            => await _orderQueries.GetOrderCount(status);

        [HttpPost("confirmOrder")]
        public async Task<ReturnResult> ConfrimOrder([FromBody] OrderConfirmCommand command)
            => await _mediator.Send(command);

        [HttpGet("getOrder/{id}")]
        public async Task<ReturnResult<SignleOrderInfo>> GetOrderInfo(string id)
            => await _orderQueries.GetOrderInfo(id);

        [HttpPost("updateOrder")]
        public async Task<ReturnResult> UpdateOrderTime([FromBody]UpdateOrderCommand command)
            => await _mediator.Send(command);

        [HttpGet("getOrderUsers")]
        public async Task<ReturnResult<List<OrderBuyUsers>>> GetOrderBuys()
            => await _orderQueries.GetOrderBuys();


        [HttpPost("createExtra")]
        public async Task<ReturnResult> CreateExtraOrder(CreateExtraCommand command)
            => await _mediator.Send(command);
        [HttpPost("updateExtra")]
        public async Task<ReturnResult> UpdateExtraOrder(UpdateExtraCommand command)
            => await _mediator.Send(command);

        [HttpPost("cancleExtra/{id}")]
        public async Task<ReturnResult> CacleExtraOrder(string id)
        {
            var com = new CancleExtraCommand();
            com.AddId(id);
            return await _mediator.Send(com);
        }

        [HttpDelete("deleteExtra")]
        public async Task<ReturnResult> DeleteExtra([FromBody] string[] ids)
        {
            var com = new DeleteExtraCommand();
            com.AddIds(ids);
            return await _mediator.Send(com);
        }

        [HttpPost("getExtraPage")]
        public async Task<ReturnResult<List<ExtraOrderDto>>> GetExtraPage(ExtraSearch search)
            => await _extraOrderQueries.GetExtraPage(search);

        [HttpPost("getOrderCusPage")]
        public async Task<ReturnResult<List<CustomerOrderDto>>> GetOrderCusPage(CusSearh search)
            => await _orderQueries.GetOrderCusPage(search);
        [HttpPost("exportOrderCus")]
        public async Task<IActionResult> ExportOrderCus()
        {
           var stram = await   _orderQueries.ExportOrderCus();
           return new FileStreamResult(stram, "application/vnd.ms-excel");
        }
        [HttpPost("exportHeightCus")]
        public async Task<FileResult> ExportHeightCus([FromBody]ExportSearch search)
        {
            var data = await _orderQueries.ExportHeightCus(search);
            return new FileContentResult(data, "application/vnd.ms-excel");
        }
        
        [HttpPost("getOrderByUser")]
        public async Task<ReturnResult<List<OrderInfoDto>>> GetSignOrderUserId([FromBody]UserOrderSearch search)
            => await _orderQueries.GetSignOrderUserId(search);

        
        [HttpGet("getDetailOrder/{orderId}")]
        public async Task<ReturnResult<List<OrderDetailDto>>> GetSignOrderUserDetail(string orderId)
            => await _orderQueries.GetSignOrderUserDetail(orderId);
    }
}