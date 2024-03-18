using System.Collections;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace LCPC.Domain.Queries;

public class ProductQueries : IProductQueries
{
    private readonly IProdcutRepository _prodcutRepository;
    private readonly ISqlDapper _sqlDapper;
    private readonly IMemoryCache _memoryCache;
    private readonly UserHelper _userHelper;

    public ProductQueries(IProdcutRepository prodcutRepository, ISqlDapper sqlDapper,IMemoryCache memoryCache,UserHelper userHelper)
    {
        _prodcutRepository = prodcutRepository;
        _sqlDapper = sqlDapper;
        _memoryCache = memoryCache;
        _userHelper = userHelper;
    }

    public async Task<ReturnResult<List<ProductDto>>> GetProductPages(ProductSearch search)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser",_userHelper.LoginName);
        #region 构建查询条件

        if (!string.IsNullOrWhiteSpace(search.Remark))
        {
            sb.Append(" and a.Remark  like @Remark ");
            hs.Add("Remark", "%" + search.Remark + "%");
        }

        if (!string.IsNullOrWhiteSpace(search.ProductModel))
        {
            sb.Append(" and ProductModel  like @ProductModel ");
            hs.Add("ProductModel", "%" + search.ProductModel + "%");
        }

        if (!string.IsNullOrWhiteSpace(search.KeyWord))
        {
            sb.Append(" and  (ProductName  like @ProductName or upper(NameSpell)  like @NameSpell )");
            hs.Add("ProductName", "%" + search.KeyWord + "%");
            hs.Add("NameSpell","%"+search.KeyWord.ToUpper()+"%");
        }

        if (!string.IsNullOrWhiteSpace(search.SupileName))
        {
            sb.Append(" and d.SupName  like @SupName ");
            hs.Add("SupName", "%" + search.SupileName + "%");
        }

        if (!string.IsNullOrWhiteSpace(search.CateId))
        {
            sb.Append(" and CateId=@CateId ");
            hs.Add("CateId", search.CateId);
        }

        #endregion

        string countSql = @"select count(a.Id)   from ProductInfo a  left join (select  Id,DicCode,DicName   from SystemDicInfo  where Enable=1  and DicType=1)  b on 
a.UnitId   =b.Id  left  join CateInfo c 
on a.CateId  = c.Id   left  join SupplierInfo d
on a.SupilerId = d.Id where 1=1  and  a.CreateUser=@CreateUser " + sb.ToString();
        long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        int start = (search.PageIndex - 1) * search.PageSize;
        hs.Add("start", start);
        hs.Add("end", search.PageSize);
        string pageSql =
            $@"select a.Id,a.ProductCode,a.ProductName,a.ProductModel,d.SupName, a.ConversionRate,a.InventoryCount,a.InitialCost,a.Purchase,
a.SellPrice,a.Wholesale,a.MaxStock,a.MinStock,a.Remark,a.Enable,b.DicCode as UnitName ,c.CateName,a.CateId,a.UnitId,a.SupilerId   from ProductInfo a left  
    join (select  Id,DicCode,DicName   from SystemDicInfo  where Enable=1  and DicType=1)  b on 
a.UnitId   =b.Id  left  join CateInfo c 
on a.CateId  = c.Id  left   join SupplierInfo d
on a.SupilerId = d.Id
where 1=1  and a.CreateUser=@CreateUser {sb.ToString()}
order by a.CreateTime   desc 
OFFSET @start
ROWS  FETCH  NEXT @end  ROWS  ONLY  ";
        var data = await _sqlDapper.QueryAsync<ProductDto>(pageSql, hs);
        var result = new ReturnResult<List<ProductDto>>(true, data, "分页获取商品成功")
        {
            TotalCount = count
        };
        return await Task.FromResult(result);
    }

    public async Task<ReturnResult<List<ProductCatesDto>>> GetProductCates()
    {
        var list = await _prodcutRepository.GetProductCates();
        return new ReturnResult<List<ProductCatesDto>>(true, list, "获取统计数据成功");
    }

    public async Task<ReturnResult<List<ProduceSellDto>>> GetProductSellPage(DataSearch search)
    {
        int sart = (search.PageIndex - 1) * search.PageSize;
        int end = search.PageSize;
        string whereStr = string.Empty;
        Hashtable hs = new Hashtable();
        hs.Add("@CreateUser",_userHelper.LoginName);
        if (!string.IsNullOrWhiteSpace(search.KeyWord))
        {
            whereStr = "and ProductName like @ProductName";
            hs.Add("ProductName", "%" + search.KeyWord + "%");
        }

        string countSql = @$"select count(a.Id) from ProductInfo a left join UnitInfo b 
         on a.UnitId  = b.Id  where 1=1  and a.CreateUser=@CreateUser {whereStr}";
        long totalCount = await _sqlDapper.QueryLongCountAsync(countSql, hs);
        hs.Add("Start", sart);
        hs.Add("End", end);
        string pageSql = @$"select a.ProductCode,a.ProductModel,a.ProductName,b.DicCode as UnitName,
     a.InventoryCount,a.SellPrice from ProductInfo a left join
         (select  Id,DicCode,DicName   from SystemDicInfo  where Enable=1  and DicType=1) b 
         on a.UnitId  = b.Id  where 1=1  and a.CreateUser=@CreateUser {whereStr} order by a.ProductCode desc offset @Start
        rows fetch  next @End rows  only";
        var data = await _sqlDapper.QueryAsync<ProduceSellDto>(pageSql, hs);
        return new ReturnResult<List<ProduceSellDto>>(true, data)
        {
            TotalCount = totalCount,
            Success = true,
            Data = data
        };
    }

    public async Task<ReturnResult> CheckProductCount(List<ProductCheck> checks)
    {
        StringBuilder sb = new StringBuilder();
        var arrys = checks.Select(x => x.ProductCode)
            .ToList();
        var list = _prodcutRepository.GetQueryable
            .Select(d => new
            {
                ProductName = d.ProductName,
                InventoryCount = d.InventoryCount,
                ProductCode = d.ProductCode,
                ProductModel = d.ProductModel
            })
            .Where(d => arrys.Contains(d.ProductCode))
            .ToList();
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            var model = checks.FirstOrDefault(x => x.ProductCode == item.ProductCode);
            if (model != null && model.Count > item.InventoryCount)
                sb.AppendLine( $"{item.ProductName}({item.ProductModel})库存不足，无法下单，剩余库存 [{item.InventoryCount}]");
        }

        var result = new ReturnResult(string.IsNullOrWhiteSpace(sb.ToString()), null, sb.ToString());
        return await Task.FromResult(result);
    }

    public async Task<ReturnResult<List<ProductForInpush>>> GetEnableProdcutSqls(string name = "")
    {
        var cacher = _memoryCache.Get<List<ProductForInpush>>("product");
        cacher = null;
        if (cacher != null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = cacher
                    .Where(d => d.ProductName.Contains(name))
                    .ToList();
                return new ReturnResult<List<ProductForInpush>>(true, result, "产品获取成功");
            }
           
            return new ReturnResult<List<ProductForInpush>>(true, cacher, "产品获取成功");
        }
        Hashtable hs = new Hashtable();
        string sql = @"select  a.ProductName,a.Id,a.ProductCode,a.ProductModel,a.InventoryCount,
        a.Purchase,a.InitialCost,a.Wholesale,a.SellPrice, b.CateName,c.DicCode as UnitName,a.CateId,a.UnitId 
      from ProductInfo a join CateInfo b 
          on a.CateId = b.Id join  (select  Id,DicCode,DicName   from SystemDicInfo  where Enable=1  and DicType=1) c  
              on a.UnitId = c.Id where a.Enable = 1 and a.CreateUser=@CreateUser";
        hs.Add("@CreateUser",_userHelper.LoginName);
        if (!string.IsNullOrWhiteSpace(name))
        {
            sql += "and a.ProductName like @ProductName";
            hs.Add("@ProductName", "%" + name + "%");
        }
        var data = await _sqlDapper.QueryAsync<ProductForInpush>(sql, hs);
        if (string.IsNullOrWhiteSpace(name))
            _memoryCache.Set("product", data);
        return new ReturnResult<List<ProductForInpush>>(true, data, "产品获取成功");
    }
}