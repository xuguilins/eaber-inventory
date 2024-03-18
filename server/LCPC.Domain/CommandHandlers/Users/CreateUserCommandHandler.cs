using System;

namespace LCPC.Domain.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreatUserCommand, ReturnResult>
    {
        private readonly IUserInfoRepository _userRepository;
        private readonly UserHelper _userHelper;
        public CreateUserCommandHandler(IUserInfoRepository userInfoRepository,UserHelper userHelper)
        {
            _userRepository = userInfoRepository;
            _userHelper = userHelper;
        }

        public async Task<ReturnResult> Handle(CreatUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindEntity(x => x.UserName.Equals(request.Name));
            if (user != null && user.Enable)
                throw new Exception("User already exists");
            var model = new UserInfo
            {
                UserAddress = request.Address,
                UserName = request.Name,
                UserPass = request.Pass,
                Remark = request.Remark,
                Enable = request.Enable,
                UserTel = request.Tel,
                CreateUser = _userHelper.LoginName
                
            };
            await _userRepository.AddAsync(model);
            var result =  await _userRepository.UnitOfWork.SaveChangesAsync();
            return result>0
            ? new ReturnResult(true,null,"用户创建成功")
            : new ReturnResult(false,null,"用户创建失败");
        }
    }
}

