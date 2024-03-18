using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ReturnResult>
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public DeleteUserCommandHandler(IUserInfoRepository userInfoRepository) {
            _userInfoRepository = userInfoRepository;
        }
        public async Task<ReturnResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            Func<UserInfo, bool> predicate = x =>  request.Ids.Contains(x.Id);
            var users =  await _userInfoRepository.GetEntitiesAsync(x=> request.Ids.Contains(x.Id));
            await  _userInfoRepository.RemoveAsync(users);
            var result = await _userInfoRepository.UnitOfWork.SaveChangesAsync();
            return result>0
              ? new ReturnResult(true,null,"数据删除成功")
              : new ReturnResult(false,null,"数据删除失败");

        }
    }
}