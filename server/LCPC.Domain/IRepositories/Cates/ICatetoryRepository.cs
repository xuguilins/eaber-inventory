namespace LCPC.Domain.IRepositories;

public interface ICatetoryRepository:IRepository<CateInfo>
{
    Task<ReturnResult<List<CateInfoDto>>> GetCatePages(DataSearch search);
}