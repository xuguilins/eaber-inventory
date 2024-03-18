using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateUserCommand : IRequest<ReturnResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public bool Enable { get; set; }
        public string Remark { get; set; }
    }
}