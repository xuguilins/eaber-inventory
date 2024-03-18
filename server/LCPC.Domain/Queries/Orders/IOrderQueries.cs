namespace LCPC.Domain.Queries;

public interface IOrderQueries:IScopeDependecy
{
    Task<ReturnResult<List<OrderInfoDto>>> GetOrderPage(OrderSearch search,int type);

    Task<ReturnResult<long>> GetOrderCount(OrderStatus status);

    Task<ReturnResult<SignleOrderInfo>> GetOrderInfo(string id);
    Task<ReturnResult<List<OrderBuyUsers>>> GetOrderBuys();

    Task<ReturnResult<List<CustomerOrderDto>>> GetOrderCusPage(CusSearh search);

    Task<MemoryStream> ExportOrderCus();

    Task<byte[]> ExportHeightCus(ExportSearch search);

    Task<ReturnResult<List<OrderInfoDto>>>  GetSignOrderUserId(UserOrderSearch search);

    Task<ReturnResult<List<OrderDetailDto>>> GetSignOrderUserDetail(string orderId);
}