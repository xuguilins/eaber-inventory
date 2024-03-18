namespace LCPC.Domain.Queries;

public interface IProductQueries:IScopeDependecy
{
    Task<ReturnResult<List<ProductDto>>> GetProductPages(ProductSearch search);

    Task<ReturnResult<List<ProductCatesDto>>> GetProductCates();

    Task<ReturnResult<List<ProduceSellDto>>> GetProductSellPage(DataSearch search);

    Task<ReturnResult> CheckProductCount(List<ProductCheck> checks);

    Task<ReturnResult<List<ProductForInpush>>> GetEnableProdcutSqls(string name="");
}