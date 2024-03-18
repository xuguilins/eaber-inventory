using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ReturnResult<LoginResultDto>>
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ITokenManager _tokenManager;
        public UserLoginCommandHandler(IUserInfoRepository userInfoRepository, ITokenManager tokenManager)
        { _userInfoRepository = userInfoRepository; _tokenManager = tokenManager; }
        public async Task<ReturnResult<LoginResultDto>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userInfoRepository.GetUserInfoByName(request.UserName);
            if (user == null)
                throw new Exception("用户名或密码错误");
            if (user.UserPass != request.UserPass)
                throw new Exception("用户名或密码错误");
            //create access_token
            var token = _tokenManager.CreateJsonWebToken(user.UserName);
            var refresh_Token = _tokenManager.CreateJsonWebRefashToken(user.UserName);
            var result = new ReturnResult<LoginResultDto>(true, new LoginResultDto { Access_Token = token, Refresh_Token = refresh_Token, Login_Name = user.UserName });
            return result;
        }
    }
}