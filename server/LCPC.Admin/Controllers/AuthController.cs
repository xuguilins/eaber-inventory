
using LCPC.Domain.Entities;
using LCPC.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;

namespace LCPC.Admin.Controllers
{
    [ApiExplorerSettings(GroupName ="认证服务")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;
        private readonly IUserInfoRepository _userRepository;
        public AuthController(ITokenManager tokenManager,IUserInfoRepository userInfoRepository) {
             _tokenManager = tokenManager;
             _userRepository = userInfoRepository;
             }
        [HttpGet("createAccessToken")]
       public async Task<ReturnResult> CreateAccessToke() {
          await _userRepository.AddAsync(new UserInfo());
           var result = new ReturnResult(true,"aa","获取身份成功");
            return await Task.FromResult(result);
       }
       [AllowAnonymous]
       [HttpGet("refreshToken")]
        public async Task<ReturnResult> RefreashToken()
        =>await  _tokenManager.RefrashToken();
      
    }
}