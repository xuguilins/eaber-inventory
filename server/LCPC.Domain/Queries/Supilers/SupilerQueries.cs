using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LCPC.Domain.Queries
{
    public class SupilerQueries : ISupilerQueries
    {
        private readonly ISupilerInfoRepository _supilerRepository;
        private readonly ISqlDapper _sqlDapper;
        private readonly UserHelper _userHelper;

        public SupilerQueries(ISupilerInfoRepository supilerInfoRepository, ISqlDapper sqlDapper,UserHelper userHelper)
        {
            _supilerRepository = supilerInfoRepository;
            _sqlDapper = sqlDapper;
            _userHelper = userHelper;
        }

        public async Task<ReturnResult<List<SupilerDto>>> GetSupilesPage(SupilerSearch search)
        {

            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser",_userHelper.LoginName);
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(search.KeyWord))
            {
                sb.Append(" and SupName like @SupName ");
                hs.Add("SupName","%"+search.KeyWord+"%");
                
            }
            if (!string.IsNullOrWhiteSpace(search.PhoneOne))
            {
                sb.Append(" and SupPhone  like @SupPhone ");
                hs.Add("SupPhone","%"+search.PhoneOne+"%");
            }
            if (!string.IsNullOrWhiteSpace(search.TelOne))
            {
                sb.Append(" and SupTel  like @SupTel ");
                hs.Add("SupTel","%"+search.TelOne+"%");
            }
            if (!string.IsNullOrWhiteSpace(search.Address))
            {
                sb.Append(" and Address  like @Address");
                hs.Add("Address","%"+search.Address+"%");
            }
            if (!string.IsNullOrWhiteSpace(search.UserOne))
            {
                sb.Append("and ProviderUser like  @ProviderUser");
                hs.Add("ProviderUser","%"+search.UserOne+"%");
            }
            string pageSql = $@"select * from (select row_number()  
            over ( order by CreateTime desc  ) as num,Id,SupNumber,SupName,SupTel,SupPhone,ProviderUser,Address,CreateTime,CreateUser,Remark,Enable,ProviderUserT,SupPhoneT,SupTelT
            from   SupplierInfo  where 1=1 and CreateUser=@CreateUser  {sb.ToString()} ) as t where t.num>=@start and t.num<=@end";
            string countsql = $@"select count(Id)    from   SupplierInfo  where 1=1 and CreateUser=@CreateUser {sb.ToString()}";
            var count = await _sqlDapper.QueryLongCountAsync(countsql, hs);
            var start = (search.PageIndex - 1) * search.PageSize+1;
            var end = search.PageIndex * search.PageSize;
            hs.Add("start",start);
            hs.Add("end",end);
            var data = await _sqlDapper.QueryAsync<SupplierInfo>(pageSql, hs);
            var newData = data.Select(x => new SupilerDto(x)).ToList();
            var result = new ReturnResult<List<SupilerDto>>(true, newData, "分页获取供应商数据成功")
            {
                TotalCount = count
            };
            return result;
        }

        public async Task<ReturnResult<SupilerDto>> GetSignleSupiler(string id)
        {
            var model = await _supilerRepository.GetByKey(id);
            if (model == null)
                throw new Exception("未找到有效的供应商");
            var dto = new SupilerDto(model);
            return new ReturnResult<SupilerDto>(true, dto, "获取指定的供应商成功");
        }

        public async Task<ReturnResult<List<SupilerSelect>>> GetSupilers()
        {
            var data = _supilerRepository.GetEntities
                .Where(d=>d.Enable)
                .Select(x => new SupilerSelect(x.SupName, x.Id))
                .ToList();
            var result = new ReturnResult<List<SupilerSelect>>(true, data, "获取可用供应商成功");
            return await Task.FromResult(result);
        }

        public async Task<ReturnResult<List<EnableSupiler>>> GetEnableSupiles()
        {
            var users = await _supilerRepository.GetEntitiesAsync(x => x.Enable);
            var dtos = users.Select(x => new EnableSupiler
            {
                SupileName =x.SupName,
                Id = x.Id,
                SupileTel = x.SupTel,
                SupileSen = x.SupTelT,
                SupileUser = x.ProviderUser,
                SupileUTEN = x.ProviderUserT,
                SupileCode = x.SupNumber

            }).ToList();
            return new ReturnResult<List<EnableSupiler>>(true, dtos, "获取可用的供应商成功");
        }
    }
}