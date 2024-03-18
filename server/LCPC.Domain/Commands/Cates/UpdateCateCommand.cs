using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class UpdateCateCommand : IRequest<ReturnResult>
    {
        public string Id { get; set; }
        public bool Enable { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string ParentId { get; set; }
    }
}