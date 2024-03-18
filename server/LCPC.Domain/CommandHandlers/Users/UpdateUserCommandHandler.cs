using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ReturnResult>
    {
        private readonly IUserInfoRepository _userinfoRepository;
        public UpdateUserCommandHandler(IUserInfoRepository userInfoRepository)
        {
            _userinfoRepository = userInfoRepository;
        }
        public async Task<ReturnResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var model = await _userinfoRepository.GetByKey(request.Id);
            if (model == null)
                throw new Exception($"未找到id “{request.Id}” 对应的用户信息");
            model.Enable = request.Enable;
            model.UserName = request.Name;
            model.UserPass = request.Pass;
            model.Remark = request.Remark;
            model.UserAddress = request.Address;
            model.UserTel = request.Tel;
            await _userinfoRepository.UpdateAsync(model);
            var result = await _userinfoRepository.UnitOfWork.SaveChangesAsync();
            return result > 0
            ? new ReturnResult(true, null, "用户修改成功")
            : new ReturnResult(false, null, "用户修改失败");

        }
    }
}