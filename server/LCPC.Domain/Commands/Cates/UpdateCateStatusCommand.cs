using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateCateStatusCommand:IRequest<ReturnResult>
    {
        public string Id { get; private  set; }
        public void AddId(string id) {
            Id = id;
        }
    }
}