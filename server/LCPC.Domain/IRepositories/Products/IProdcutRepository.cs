namespace LCPC.Domain.IRepositories;

public interface IProdcutRepository:IRepository<ProductInfo>
{
    Task<List<ProductCatesDto>> GetProductCates();

    Task<List<ProductInfo>> GetProductPages(ProductSearch search);

    Task<List<ProductInfo>> GetIncludeProduct(string[] numbers);

    Task<ProductInfo> GetProductByCode(string code);

    Task UpdateProductCount(string id, int count);
}