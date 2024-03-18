using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public interface ISupilerQueries:IScopeDependecy
    {
       Task<ReturnResult<List<SupilerDto>>> GetSupilesPage(SupilerSearch search);
       Task<ReturnResult<SupilerDto>> GetSignleSupiler(string id);

       Task<ReturnResult<List<SupilerSelect>>> GetSupilers();

       Task<ReturnResult<List<EnableSupiler>>> GetEnableSupiles();
    }
}