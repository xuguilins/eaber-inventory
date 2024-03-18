using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.QueriesDtos
{
    public class CateSearch : DataSearch
    {
        public string ParentId { get; set; }
    }
    public record CateInfoDto
    {
    
        public string Id { get;  set; }
        public string Name { get;  set; }
        public string ParentName { get;  set; }
        public string ParentId { get;  set; }
        public bool Enable { get;  set; }
        public string Remark { get;  set; }
    }
    public record CateTreeDto {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Label { get; set; }
        public string Value {get;set;}
        public string  key { get; set; }
        public List<CateTreeDto> Children { get; set;}
    }
}
