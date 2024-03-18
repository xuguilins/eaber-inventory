using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public record RuleInfoDto
    {
        public RuleInfoDto(RuleInfo info) {
            Id  = info.Id;
            Name = info.RuleName;
            RuleType = info.RuleType;
            RulePix = info.RulePix;
            IdentityNum = info.IdentityNum;
            RuleFormatter =info.Formatter;
            AppendNum  = info.RuleAppend;
            Enable = info.Enable;
            Remark = info.Remark;
        }
        public string Id { get; private set; }
         public string Name { get; private set; }
        public RuleType RuleType { get; private set; }
        public string RulePix { get; private set; }
        public int IdentityNum { get; private set; }
        public string RuleFormatter { get; private set; }
        public int AppendNum { get;  private set; }
        public bool Enable { get;private  set; }
        public string Remark { get; private set; }
    }
}