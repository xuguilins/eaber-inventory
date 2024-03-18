using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace LCPC.Domain.Queries;

public class SystemDicQueries:ISystemDicQueries
{
    private readonly ISqlDapper _sqlDapper;
    private readonly UserHelper _userHelper;
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
   public SystemDicQueries(ISqlDapper sqlDapper,UserHelper userHelper,ISystemDicInfoRepository systemDicInfoRepository)
   {
       _sqlDapper = sqlDapper;
       _userHelper = userHelper;
       _systemDicInfoRepository = systemDicInfoRepository;
   }
   public async Task<ReturnResult<List<SystemDicDto>>> GetSystemDicPage(DicSearch search)
   {
       var user = _userHelper.LoginName;
       Hashtable hs = new Hashtable();
       StringBuilder sb = new StringBuilder();
       hs.Add("@CreateUser",user);
       if (search.DicType != DicType.ALL)
       {
           hs.Add("@DicType",search.DicType);
           sb.Append(" and DicType=@DicType ");
       }
       if (!string.IsNullOrWhiteSpace(search.KeyWord))
       {
           hs.Add("@DicName","%"+search.KeyWord+"%");
           sb.Append("  and DicName like @DicName ");
       }
       
       long count = await 
           _sqlDapper.QueryLongCountAsync(
               "select count(1) from  SystemDicInfo where CreateUser=@CreateUser " + sb.ToString(), hs);
       
       string filterSql = @"select * from  SystemDicInfo where CreateUser=@CreateUser " + sb.ToString() +
                          "order by CreateTime  desc   OFFSET @start  rows     fetch next @end  rows only";
       hs.Add("@start",(search.PageIndex-1)*search.PageSize);
       hs.Add("@end",search.PageSize);
       var list = await _sqlDapper.QueryAsync<SystemDicDto>(filterSql, hs);
       var result = new ReturnResult<List<SystemDicDto>>(true, list, "分页获取系统字典成功");
       result.TotalCount = count;
       return result;
   }

   public async Task<ReturnResult<List<DicModel>>> GetDicTypes()
   {
       List<DicModel> list = new List<DicModel>();
       var data = typeof(DicType).GetMembers();
       var values = Enum.GetValues(typeof(DicType));
       Dictionary<string, int> dics = new Dictionary<string, int>();
       foreach (var item in values)   
       {
           dics.Add(item.ToString(),Convert.ToInt32(item));
       }
       foreach (MemberInfo item in data)
       {
           var v = item.GetCustomAttribute<DescriptionAttribute>();
           if (v != null)
           {
               DicModel info = new DicModel
               {
                   Name = v.Description,
                   Value = dics[item.Name]
               };
               list.Add(info);
           }
       }
       var result =new ReturnResult<List<DicModel>>(true,list);
       return await Task.FromResult(result);
   }

   public async Task<ReturnResult<List<DicModel>>> GetRuleTypes()
   {
       List<DicModel> list = new List<DicModel>();
       var data = typeof(RuleType).GetMembers();
       var values = Enum.GetValues(typeof(RuleType));
       Dictionary<string, int> dics = new Dictionary<string, int>();
       foreach (var item in values)   
       {
           dics.Add(item.ToString(),Convert.ToInt32(item));
       }
       foreach (MemberInfo item in data)
       {
           var v = item.GetCustomAttribute<DescriptionAttribute>();
           if (v != null)
           {
               DicModel info = new DicModel
               {
                   Name = v.Description,
                   Value = dics[item.Name]
               };
               list.Add(info);
           }
       }
       var result =new ReturnResult<List<DicModel>>(true,list);
       return await Task.FromResult(result);
   }
   public async Task<ReturnResult<List<UnitInfoDto>>> GetUnits()
   {

       var data = _systemDicInfoRepository
           .GetEntities.Where(x => x.Enable && x.CreateUser.Equals(_userHelper.LoginName) && x.DicType == DicType.UNIT)
           .Select(v => new UnitInfoDto
           {
               Id = v.Id,
               Name = v.DicName,
               Enable = v.Enable,
               Remark = v.Remark
           }).ToList();
           var result = new ReturnResult<List<UnitInfoDto>>(true, data, "获取单位成功");
           return await Task.FromResult(result);
       
   }

   public async Task<ReturnResult<List<SysDicData>>> GetDics(DicType type)
   {
       var data = _systemDicInfoRepository
           .GetEntities.Where(x => x.Enable && x.CreateUser.Equals(_userHelper.LoginName) && x.DicType == type)
           .Select(v => new SysDicData 
           {
               Id = v.Id,
               Name = v.DicName,
               Value = v.DicCode
           }).ToList();
       var result = new ReturnResult<List<SysDicData>>(true, data, "获取指定的类型数据成功");
       return await Task.FromResult(result);
   }
   public async Task<ReturnResult<List<EnablePayInfoDto>>> GetPays()
   {
       var data = await _systemDicInfoRepository
           .GetEntitiesAsync(x => x.Enable && x.DicType == DicType.PAY);
       var dtos = data.Select(m => new EnablePayInfoDto
       {
           Id = m.Id,
           PayName = m.DicCode

       }).ToList();
       return new ReturnResult<List<EnablePayInfoDto>>(true, dtos, "获取可用的支付方式成功");
   }
   public async Task test()
   {
       await Task.CompletedTask;
   }
}