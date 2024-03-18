using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public interface IUserInfoQueries:IScopeDependecy
    {
        Task<ReturnResult<UserInfoDto>> QueryUserInfo(string userId);
    }
}