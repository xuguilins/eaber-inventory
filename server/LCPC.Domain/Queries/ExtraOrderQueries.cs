using System.Collections;
using System.Text;

namespace LCPC.Domain.Queries;

public class ExtraOrderQueries:IExtraOrderQueries
{
   private readonly ISqlDapper _sqlDapper;
   public ExtraOrderQueries(ISqlDapper sqlDapper)
   {
       _sqlDapper = sqlDapper;
   }
   public async Task<ReturnResult<List<ExtraOrderDto>>> GetExtraPage(ExtraSearch search)
   {
       Hashtable hs = new Hashtable();
       StringBuilder sb = new StringBuilder();
       if (search.ExtraType != ExtraType.ALL)
       {
           sb.Append(" and ExtraType=@ExtraType ");
           hs.Add("@ExtraType",search.ExtraType);
       }

       if (!string.IsNullOrWhiteSpace(search.KeyWord))
       {
           sb.Append(" and   TypeName like @TypeName");
           hs.Add("@TypeName","%"+search.KeyWord+"%");
       }
       string countSql = @"select count(1) from ExtraOrder where 1=1 "+sb.ToString();
       long count = await _sqlDapper.QueryLongCountAsync(countSql, hs);
       int start = (search.PageIndex - 1) * search.PageSize;
       int end = search.PageSize;
       string filestrSql = @"select  Id,ExtraType,Price,Remark,Enable,OrderCode,TypeName
        from ExtraOrder
order by  CreateTime
OFFSET @start  rows     fetch next @end  rows only";
       hs.Add("@start",start);
       hs.Add("@end",end);

       var list = await _sqlDapper.QueryAsync<ExtraOrderDto>(filestrSql, hs);
       var result = new ReturnResult<List<ExtraOrderDto>>(true, list, "分页获取其它支出成功");
       result.TotalCount = count;
       return result;

   }
}