using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateRuleCommand : IRequest<ReturnResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RuleType RuleType { get; set; }
        public string RulePix { get; set; }
        public int IdentityNum { get; set; }
        public string RuleFormatter { get; set; }
        public int AppendNum { get; set; }
        public bool Enable { get; set; }
        public string Remark { get; set; }
    }
}