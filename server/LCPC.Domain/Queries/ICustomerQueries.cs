namespace LCPC.Domain.Queries;

public interface ICustomerQueries:IScopeDependecy
{
    Task<ReturnResult<List<CustomerDto>>> GetCustomerPages(DataSearch search);
}