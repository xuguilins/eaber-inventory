using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LCPC.Domain.Services
{
    public class RuleManager : IRuleManager
    {
        private readonly ISqlDapper _sqlDapper;
        private readonly UserHelper _userHelper;
        public RuleManager(ISqlDapper sqlDapper,UserHelper userHelper)
        {
            _sqlDapper = sqlDapper;
            _userHelper = userHelper;
        } 
        public async Task<string> getNextRuleNumber(RuleType ruleType)
        {
            // 查询当前规则
            string sql = "select CreateTime,Formatter,IdentityNum,RuleType, RuleAppend,NowValue,RulePix from RuleInfo where RuleType=@RuleType and  Enable = 1 and CreateUser=@CreateUser";
            var rule = await _sqlDapper.QueryFirstAsync<RuleInfo>(sql, new { RuleType = ruleType,CreateUser = _userHelper.LoginName });
            if (rule == null)
                throw new Exception("无效的规则，请到【编码管理】中检查");
            var ruleCreatTime = rule.CreateTime;
            var now = DateTime.Now;
            if(rule.Formatter == "yyyyMM")
            {
                if ((ruleCreatTime.Year != now.Year) || (ruleCreatTime.Month != now.Month))
                {
                    await restNowValue(ruleType);
                    rule.NowValue = 0;
                }
            }
            else
            {
                if ((ruleCreatTime.Year != now.Year) || (ruleCreatTime.Month != now.Month) || (ruleCreatTime.Day != now.Day))
                {
                    await restNowValue(ruleType);
                    rule.NowValue = 0;
                }
            }
          
            StringBuilder sb = new StringBuilder();
            sb.Append(rule.RulePix);
            string formatter = DateTime.Now.ToString(rule.Formatter);
            sb.Append(formatter);
            string endValue = (rule.NowValue + rule.IdentityNum).ToString();
            await updateNowValue(ruleType, endValue);
            string appdStr = endValue.PadLeft(rule.RuleAppend, '0');
            sb.Append(appdStr);
            return sb.ToString();
        }


        public async Task<string[]> createMuchNumber(RuleType ruleType, int number = 100)
        {
            string[] list = new string[number];
            // 查询当前规则
            string sql = "select CreateTime,Formatter,IdentityNum,RuleType, RuleAppend,NowValue,RulePix from RuleInfo where RuleType=@RuleType and  Enable = 1 and CreateUser=@CreateUser";
            var rule = await _sqlDapper.QueryFirstAsync<RuleInfo>(sql, new { RuleType = ruleType ,CreateUser = _userHelper.LoginName });
            if (rule == null)
                throw new Exception("无效的规则，请到【编码管理】中检查");
            var ruleCreatTime = rule.CreateTime;
            var now = DateTime.Now;
            if ((ruleCreatTime.Year != now.Year) || (ruleCreatTime.Month != now.Month) || (ruleCreatTime.Day != now.Day)) {
                await restNowValue(ruleType);
                rule.NowValue = 0;
            }
            string strNumbers = string.Empty;
            int nowValue = rule.NowValue;
            for (int i = 0; i < number; i++)
            {
                nowValue += rule.IdentityNum;
                string nowStr = nowValue.ToString();
                strNumbers =
                    $"{rule.RulePix}{DateTime.Now.ToString(rule.Formatter)}" +
                    $"{nowStr.PadLeft(rule.RuleAppend, '0')}";
                list[i] = strNumbers;
              
            }
           await updateNowValue(ruleType, nowValue.ToString());
            return list;
        }

        public async Task restNowValue(RuleType ruleType)
        {
            string sql = "UPDATE RuleInfo SET NowValue=0, CreateTime=getdate()   WHERE RuleType=@RuleType and CreateUser=@CreateUser";
            await _sqlDapper.UpdateAsync(sql, new { RuleType = ruleType,CreateUser = _userHelper.LoginName });
            await Task.CompletedTask;
        }
        private async Task updateNowValue(RuleType ruleType, string num)
        {
            string sql = "UPDATE RuleInfo SET NowValue=@NowValue, CreateTime=getdate()   WHERE RuleType=@RuleType and CreateUser=@CreateUser";
            await _sqlDapper.UpdateAsync(sql, new { RuleType = ruleType, NowValue = num,CreateUser = _userHelper.LoginName });
            await Task.CompletedTask;
        }
    }
}