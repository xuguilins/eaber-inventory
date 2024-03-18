namespace LCPC.Domain.Queries;

public interface IExtraOrderQueries:IScopeDependecy
{
    Task<ReturnResult<List<ExtraOrderDto>>> GetExtraPage(ExtraSearch search);
}