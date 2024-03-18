using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Commands
{
    public class DeleteCateCommand : IRequest<ReturnResult>
    {
        public string[] Ids { get; private set; }
        public void AddIds(string[] _ids)
        {
            Ids = _ids;
        }
    }
}