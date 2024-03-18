using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Share.Response
{
    public class DataSearch
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string KeyWord { get; set; }
    }
}