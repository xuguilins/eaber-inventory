using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCPC.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace LCPC.Domain.Queries
{
    public class CateInfoQueries : ICateInfoQueries
    {
        private readonly ICatetoryRepository _catetoryRepository;
        private readonly ISqlDapper _sqlDapper;
        private IHubContext<HubClient, IHubClient> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserHelper _userHelper;
        public CateInfoQueries(ICatetoryRepository catetoryRepository, 
            ISqlDapper sqlDapper,IHubContext<HubClient, IHubClient> hubContext,
            IHttpContextAccessor httpContextAccessor,
            UserHelper userHelper
            
             
            )
        {
            _catetoryRepository = catetoryRepository;
            _sqlDapper = sqlDapper;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _userHelper = userHelper;

        }
        public async Task<ReturnResult<List<CateInfoDto>>> GetPageCates(CateSearch search)
        {
            StringBuilder sb = new StringBuilder();
            Hashtable hs = new Hashtable();
            hs.Add("@CreateUser",_userHelper.LoginName);
            int start = (search.PageIndex - 1) * search.PageSize + 1;
            int end = search.PageIndex * search.PageSize;
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(search.KeyWord))
            {
                where = $" and   b.CateName like @CateName";
                hs.Add("@CateName","%"+search.KeyWord+"%");
            }
            if (!string.IsNullOrWhiteSpace(search.ParentId))
            { 
                where += " and b.ParentId=@ParentId";
                hs.Add("@ParentId",search.ParentId);
            }
               
            string totalSql = $"select count(Id) from CateInfo b  where 1=1 and b.CreateUser=@CreateUser {where}";
            int total = await _sqlDapper.QueryCountAsync(totalSql,hs);
            hs.Add("@start",start);
            hs.Add("@end",end);
            string sql = @$"select t.Id,t.CateName as Name, (case
            when t.ParentName is null
                then '全部'
            else t.ParentName end
           ) as ParentName ,t.ParentId,t.Remark,t.Enable from (select row_number()  
            over ( order by b.CreateTime desc  ) as num, b.Id,b.CateName,a.CateName as ParentName,a.Id as ParentId,b.Remark,b.Enable
            from CateInfo a right  join CateInfo b
            on a.Id = b.ParentId where  1=1 and (b.CreateUser=@CreateUser  or a.CreateUser=@CreateUser ) {where} ) as t where t.num>=@start and t.num<=@end";
            var data = await _sqlDapper.QueryAsync<CateInfoDto>(sql, hs);
            var result = new ReturnResult<List<CateInfoDto>>(true, data.ToList(), "分页获取分类成功");
            result.TotalCount = total;
            return result;
        }
        public async Task<ReturnResult<List<CateTreeDto>>> GetTreeCates(bool enable)
        {
            int valueData = enable ? 1 : 0;
            string whereStr = string.Empty;
            Hashtable hs = new Hashtable();
            if (enable)
            {               
                whereStr=" and b.Enable =@enable";
                hs.Add("@enable",valueData);
            }
            hs.Add("@CreateUser",_userHelper.LoginName);
            string sql = @"select b.CateName as Label,b.Id as Value,b.ParentId ,b.Enable ,b.Id as Id  from CateInfo a right  join  CateInfo  b
on a.Id = b.ParentId where  (a.CreateUser=@CreateUser or b.CreateUser=@CreateUser) " + whereStr ;
            var allList = await _sqlDapper.QueryAsync<CateTreeDto>(sql,hs);
            var list =  allList.ToList();
            //增加全部数据
          
            list.ForEach(item =>
            {
                if (string.IsNullOrWhiteSpace(item.ParentId))
                    item.ParentId = "1000";
            });
            list.Add(new CateTreeDto()
            {
                Id = "1000",
                Label = "全部",
                Value = "1000",
                key = "1000",
                Children = new List<CateTreeDto>()
            });
            var data = buildTreeDtos(list);
           
            var result = new ReturnResult<List<CateTreeDto>>(true, data, "获取可用分类成功");
            return result;

        }
        public async Task<ReturnResult<CateInfoDto>> GetCateInfo(string id)
        {
            var model = await _catetoryRepository.FindEntity(x => x.Id == id);
            if (model == null)
                throw new Exception("未找到有效的数据");
            CateInfoDto info = new CateInfoDto
            {
                Id = model.Id,
                Enable = model.Enable,
                Name = model.CateName,
                ParentId = model.ParentId,
                Remark = model.Remark,
            };
            if (!string.IsNullOrWhiteSpace(model.ParentId))
            {
                var parentInfo = await _catetoryRepository.FindEntity(d => d.Id == model.ParentId);
                if (parentInfo != null)
                {
                    info.ParentName = parentInfo.CateName;
                }
            }
            var result = new ReturnResult<CateInfoDto>(true, info, "获取指定的分类成功");
            return result;
        }
        private List<CateTreeDto> buildTreeDtos(List<CateTreeDto> dtos, string parentId = null)
        {
            List<CateTreeDto> list = new List<CateTreeDto>();
            List<CateTreeDto> data = dtos.Where(x => x.ParentId == parentId).ToList();
            if (data.Any())
            {
                data.ForEach(item =>
                {
                    list.Add(new CateTreeDto
                    {
                        Id = item.Id,
                        key = item.Id,
                        Label = item.Label,
                        Value = item.Value,
                        ParentId = item.ParentId,
                        Children = buildTreeDtos(dtos, item.Id)
                    });
                });
            }
            return list;
        }

    }
}