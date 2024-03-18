using LCPC.Domain;
using LCPC.Domain.QueriesDtos;
using LCPC.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LCPC.Infrastructure.Repositories;

public class ProdcutRepository:Repository<ProductInfo>,IProdcutRepository
{

    private readonly AdminDbContext _adminDbContext;
    private readonly ISqlDapper _sqlDapper;
    private readonly UserHelper _userHelper;
    public ProdcutRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
        _adminDbContext = context;
        _sqlDapper = serviceProvider.GetRequiredService<ISqlDapper>();
        _userHelper = serviceProvider.GetRequiredService<UserHelper>();
    }

    public  async Task<List<ProductCatesDto>> GetProductCates()
    {
        string sql =
            "select a.Id,a.CateName, (   case when  a.ParentId is null           then null else a.ParentId end   ) as ParentId,count(b.Id) as COUNTVALUE  from CateInfo a left  join  ProductInfo b " +
            "on a.Id  =b.CateId  where a.Enable =  1  and a.CreateUser=@CreateUser group by  a.Id, a.CateName, a.ParentId; ";
        var data = await _sqlDapper.QueryAsync<ProductCate>(sql,new {CreateUser = _userHelper.LoginName});
        data.ForEach(item =>
        {
            if (string.IsNullOrWhiteSpace(item.ParentId))
                item.ParentId = null;
        });
        var list =resoleTree(data.ToList(),  null);
        return list;
    }

    public async Task<List<ProductInfo>> GetProductPages(ProductSearch search)
    {
        var data = _adminDbContext.Set<ProductInfo>()
            .Where(d=>d.CreateUser.Equals(_userHelper.LoginName))
            .Include(v => v.Cate)
   
            .WhereIf(!string.IsNullOrWhiteSpace(search.KeyWord), x => x.ProductName.Contains(search.KeyWord))
            .WhereIf(!string.IsNullOrWhiteSpace(search.Remark), x => x.Remark.Contains(search.Remark))
            .WhereIf(!string.IsNullOrWhiteSpace(search.ProductModel), x => x.ProductModel.Contains(search.ProductModel))
            .WhereIf(!string.IsNullOrWhiteSpace(search.CateId),x=>x.CateId.Equals(search.CateId))
            .OrderBy(v => v.CreateTime)
            .Skip((search.PageIndex - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToList();
        return await Task.FromResult(data);
    }

    public async Task<ProductInfo> GetProductByCode(string code)
    {
       return  await  _adminDbContext.Set<ProductInfo>().FirstOrDefaultAsync(d => d.ProductCode == code && d.Enable);
    }
    public async Task<List<ProductInfo>> GetIncludeProduct(string[] numbers)
    {
        var data = await _adminDbContext.Set<ProductInfo>()
            .Where(x => numbers.Contains(x.ProductCode) && x.Enable && x.CreateUser.Equals(_userHelper.LoginName))
            .ToListAsync();
        return data;
    }

    public async Task UpdateProductCount(string id, int count)
    { 
        await _adminDbContext.Set<ProductInfo>()
            .Where(d=>d.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(m => m.InventoryCount, count)
            );
        await Task.CompletedTask;
    }
    private List<ProductCatesDto> resoleTree(List<ProductCate> productInfos,string parentId=null)
    {
        int value = 0;
        List<ProductCatesDto> list = new List<ProductCatesDto>();
        var cates = productInfos.Where(x => x.ParentId == parentId).ToList();
        foreach (var item in cates)
        {
            var info = new ProductCatesDto
            {
                CateId = item.Id,
                CateName = item.CateName+$"({item.COUNTVALUE})",
                Count = item.COUNTVALUE
            };
           
            info.Children  = resoleTree(productInfos, item.Id);
            info.Count += info.Children.Sum(d => d.Count);
            info.CateName = item.CateName + $"({info.Count})";
            list.Add(info);
        }

        value = 0;
        return list;
    }

    private int resovleCount(List<ProductInfo> productInfos,string cateId)
    {
        List<int> list = new List<int>();
        list.Add(productInfos.Count(v=>v.CateId == cateId));
        return list.Sum();
    }
}