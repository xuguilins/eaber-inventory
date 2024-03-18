using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public record LoginResultDto
    {
        public string Access_Token { get; set; }
        public string Login_Name { get; set; }
        public string Refresh_Token { get; set; }
    }
}