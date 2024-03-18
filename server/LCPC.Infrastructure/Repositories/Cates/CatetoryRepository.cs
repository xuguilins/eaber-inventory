using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.QueriesDtos;
using LCPC.Share.Response;
using Microsoft.Extensions.DependencyInjection;

namespace LCPC.Infrastructure.Repositories
{
    public class CatetoryRepository : Repository<CateInfo>, ICatetoryRepository
    {
        private readonly AdminDbContext _context;
        private readonly ISqlDapper _sqlDapper;
        public CatetoryRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
            _context = context;
            _sqlDapper = serviceProvider.GetRequiredService<ISqlDapper>();
        }

        public async Task<ReturnResult<List<CateInfoDto>>> GetCatePages(DataSearch search)
        {
  
            int start = (search.PageIndex - 1) * search.PageSize + 1;
            int end = search.PageIndex * search.PageSize;
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(search.KeyWord))
                where = $" where  b.CateName like '%{search.KeyWord}%'";

            string totalSql = $"select count(Id) from CateInfo b  {where}";
            int total = await _sqlDapper.QueryCountAsync(totalSql);
            string sql = @$"select t.Id,t.CateName as Name,t.ParentName,t.ParentId,t.Remark,t.Enable from (select row_number()  
            over ( order by b.Id ) as num, b.Id,b.CateName,a.CateName as ParentName,a.Id as ParentId,b.Remark,b.Enable
            from CateInfo a right  join CateInfo b
            on a.Id = b.ParentId {where} ) as t where t.num>=@start and t.num<=@end";
            var data = await _sqlDapper.QueryAsync<CateInfoDto>(sql, new { start, end});
            var result = new ReturnResult<List<CateInfoDto>>(true, data.ToList(), "分页获取分类成功");
            result.TotalCount =total;
            return result;

        }
    }
}