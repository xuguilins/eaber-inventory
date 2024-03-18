using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public interface ICateInfoQueries:IScopeDependecy
    {
        Task<ReturnResult<List<CateInfoDto>>> GetPageCates(CateSearch search);

        Task<ReturnResult<List<CateTreeDto>>> GetTreeCates(bool enable);

        Task<ReturnResult<CateInfoDto>>  GetCateInfo(string id);
    }
}