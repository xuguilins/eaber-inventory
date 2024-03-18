namespace LCPC.Domain.IServices;

public interface IHomeService : IScopeDependecy
{
    Task<ReturnResult<HomeCardDto>> GetHomeStatic();
    Task<ReturnResult<ColumnCardDto>> GetColumns(ColumnsDto dto);

    Task<ReturnResult<SystemInfoDto>> GetSystem();

    Task<ReturnResult<OrderCardDto>> GetOrderColumns(ColumnsDto dto);

    Task<ReturnResult<List<HeightProduct>>> GetHeightProduct(ColumnsDto dto);

    Task<ReturnResult> UpdateProdcutName();
}