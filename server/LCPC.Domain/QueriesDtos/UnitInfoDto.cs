using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public record UnitInfoDto
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool  Enable { get; set; }
        public string Remark { get; set; }
    }
}