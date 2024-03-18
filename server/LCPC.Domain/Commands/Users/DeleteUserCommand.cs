using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class DeleteUserCommand:IRequest<ReturnResult>
    {
        public DeleteUserCommand(string[] ids) {
            Ids = ids;
        }
        public string[]  Ids { get; private set; }
    }
}