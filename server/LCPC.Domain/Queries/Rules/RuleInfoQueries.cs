using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public class RuleInfoQueries : IRuleInfoQueries
    {
        private readonly IRuleInfoRepository _ruleInfoRepository;
        private readonly IRuleManager _ruleManager;
        private readonly UserHelper _userHelper;
        public RuleInfoQueries(IRuleInfoRepository ruleInfoRepository,IRuleManager ruleManager,UserHelper userHelper) {
            _ruleInfoRepository = ruleInfoRepository;
            _ruleManager = ruleManager;
            _userHelper = userHelper;
        }
        public async Task<ReturnResult<string>> GetNextRuleCode(RuleType ruleType)
        {
            var data = await _ruleManager.getNextRuleNumber(ruleType);
            return new ReturnResult<string>(true,data,"获取下一个编码成功");

        }

        public async Task<ReturnResult<List<RuleInfoDto>>> GetRulePages(DataSearch search)
        {
         
            long totalCount = await _ruleInfoRepository.GetEntities
                .Where(d=>d.CreateUser.Equals(_userHelper.LoginName))
                .CountAsyncIf(!string.IsNullOrWhiteSpace(search.KeyWord),x=>x.RuleName.Contains(search.KeyWord));
            int start = (search.PageIndex - 1) * search.PageSize;
            var data = _ruleInfoRepository.GetEntities
                .Where(d=>d.CreateUser.Equals(_userHelper.LoginName))
                .WhereIf(!string.IsNullOrWhiteSpace(search.KeyWord),x => x.RuleName.Contains(search.KeyWord))
            .OrderByDescending(x => x.CreateTime).Skip(start).Take(search.PageSize).Select(x => new RuleInfoDto(x))
            .ToList();
            var result = new ReturnResult<List<RuleInfoDto>>(true, data, "分页获取单位成功")
            {
                TotalCount = totalCount
            };
            return await Task.FromResult(result);
        }

        public async Task<ReturnResult<RuleInfoDto>> GetSignleRule(string id)
        {
           var model = await _ruleInfoRepository.GetByKey(id);
           if (model == null)
             throw new Exception("未找到有效的数据");
         var dto  = new RuleInfoDto(model);
         return new ReturnResult<RuleInfoDto>(true,dto,"获取指定的编码规则成功");
        }
    }
}