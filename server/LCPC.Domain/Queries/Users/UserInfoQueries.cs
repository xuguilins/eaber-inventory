using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Queries
{
    public class UserInfoQueries : IUserInfoQueries
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public UserInfoQueries(IUserInfoRepository userInfoRepository) { _userInfoRepository = userInfoRepository;}
        public async Task<ReturnResult<UserInfoDto>> QueryUserInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
               throw new Exception("用户Id不能为空");
            // 根据上下文编写用户查询
            var user  = await _userInfoRepository.GetByKey(userId);
            if (user == null)
               throw new Exception("指定的用户不存在");
            var userdto = new UserInfoDto(user.Id,user.UserName,user.UserTel,user.UserAddress,user.Remark,user.Enable);
            var result = new ReturnResult<UserInfoDto>(true,userdto,"获取指定的用户信息成功");
            return result;
                
        }
    }
}