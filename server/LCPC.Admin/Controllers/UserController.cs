using LCPC.Domain.Commands;
using LCPC.Domain.QueriesDtos;

namespace LCPC.Admin.Controllers
{
    [ApiExplorerSettings(GroupName ="用户服务")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserInfoQueries _userInfoQueries;

        public UserController(IMediator mediator, IUserInfoQueries userInfoQueries)
        {
            _mediator = mediator;
            _userInfoQueries = userInfoQueries;
        }

        [HttpPost("userlogin")]
        public async Task<ReturnResult<LoginResultDto>> UserLogin([FromBody] UserLoginCommand command)
            => await _mediator.Send(command);

        [HttpPost("createUser")]
        public async Task<ReturnResult> CreateUser([FromBody] CreatUserCommand command)
            => await _mediator.Send(command);

        [HttpPost("updateUser")]
        public async Task<ReturnResult> UpdateUser([FromBody] UpdateUserCommand command)
            => await _mediator.Send(command);

        [HttpGet("getUser/{userId}")]
        public async Task<ReturnResult<UserInfoDto>> GetUser(string userId)
            => await _userInfoQueries.QueryUserInfo(userId);

        [HttpDelete("delteUsers")]
        public async Task<ReturnResult> DeleteUsers([FromBody] string[] ids)
            => await _mediator.Send(new DeleteUserCommand(ids));
    }
}