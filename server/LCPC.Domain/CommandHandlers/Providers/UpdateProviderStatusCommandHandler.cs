using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateProviderStatusCommandHandler : IRequestHandler<UpdateProviderStatusCommand, ReturnResult>
    {
        private readonly ISupilerInfoRepository _supilerInfoRepository;
        public UpdateProviderStatusCommandHandler(ISupilerInfoRepository supilerInfoRepository)
        {
            _supilerInfoRepository = supilerInfoRepository;
        }
        public async Task<ReturnResult> Handle(UpdateProviderStatusCommand request, CancellationToken cancellationToken)
        {
            var model = await _supilerInfoRepository.GetByKey(request.Id);
            if (model == null)
                throw new Exception("未找到有效的数据");
            model.Enable = !model.Enable;
            await _supilerInfoRepository.UpdateAsync(model);
            int result =await _supilerInfoRepository.UnitOfWork.SaveChangesAsync();
            return result>0? new ReturnResult(true,null,"状态更新成功")
            :new ReturnResult(false,null,"状态更新失败");
        }
    }
}