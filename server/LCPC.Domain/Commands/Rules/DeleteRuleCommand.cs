using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class DeleteRuleCommand:IRequest<ReturnResult>
    {
        public DeleteRuleCommand() {

        }
        public void AddIds(string[] ids) {
            Ids = ids;
        }
        public string[] Ids{get;private set;}
    }
}