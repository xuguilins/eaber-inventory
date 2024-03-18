using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.IServices
{
    public interface IRuleManager:IScopeDependecy
    {
       Task<string>  getNextRuleNumber(RuleType ruleType);

       Task<string[]> createMuchNumber(RuleType ruleType, int number = 100);
       Task  restNowValue(RuleType ruleType);

    }
}