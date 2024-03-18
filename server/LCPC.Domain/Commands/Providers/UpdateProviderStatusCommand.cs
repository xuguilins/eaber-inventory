using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateProviderStatusCommand:IRequest<ReturnResult>
    {
        public void AddId(string _id) {
            Id = _id;
        }
        public string Id { get; private  set; }
    }
}