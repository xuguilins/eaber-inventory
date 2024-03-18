using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UserLoginCommand:IRequest<ReturnResult<LoginResultDto>>
    {
        public string UserName { get; set; }
        public string UserPass { get; set; }
    }
}