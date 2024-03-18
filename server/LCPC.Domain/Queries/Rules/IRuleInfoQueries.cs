using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public interface IRuleInfoQueries:IScopeDependecy
    {
        Task<ReturnResult<RuleInfoDto>> GetSignleRule(string id);

        Task<ReturnResult<List<RuleInfoDto>>>  GetRulePages(DataSearch search);

        Task<ReturnResult<string>> GetNextRuleCode(RuleType ruleType);
    }
}