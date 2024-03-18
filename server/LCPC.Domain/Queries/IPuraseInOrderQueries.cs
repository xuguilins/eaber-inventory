using System.ComponentModel;

namespace LCPC.Domain.Queries;

public interface IPuraseInOrderQueries:IScopeDependecy
{
    Task<ReturnResult< List<PuraseInOrderDto>>> GetInPurasePage(PuraseSearch search);

    Task<ReturnResult<PuraseOutDto>> GetPuraseInfo(string id);

    Task<ReturnResult<List<PurasheOutModalDto>>> GetPurashModalPage(DataSearch search);

    Task<ReturnResult<List<PurasheOutModalDetail>>> GetPurashModalDetail(string id);

    Task<ReturnResult<List<PuraseOutOrderDto>>> GetOutPurasePage(PuraseSearch search);

    Task<ReturnResult<PuraseOutOrderSigleDto>> GetSigleOutPurashInfo(string id);

    Task<ReturnResult<List<PurashInOrderDtoRecord>>> GetSupileCusPage(DataSearch search);

    Task<byte[]> ExportCusExcel();

    Task<byte[]> ExportCusHegithExcel(PurashExcelSearch search);

    Task<ReturnResult<List<PurashInOrderTabDetail>>> GetSupileCusTable(SupPuSearch search);
    Task<ReturnResult<List<PurashInOrderDetailRecord>>> GetSupileCusDetail(string Id);
}
