using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers.Cates
{
    public class UpdateCateStatusCommandHandler : IRequestHandler<UpdateCateStatusCommand, ReturnResult>
    {
        private readonly ICatetoryRepository _cateRepository;
        public UpdateCateStatusCommandHandler(ICatetoryRepository catetoryRepository)
        {
            _cateRepository = catetoryRepository;
        }
        public async Task<ReturnResult> Handle(UpdateCateStatusCommand request, CancellationToken cancellationToken)
        {
            var model = await _cateRepository.FindEntity(x => x.Id == request.Id);
            if (model == null)
                throw new Exception("未找到有效的数据");
            model.Enable = !model.Enable;
            await _cateRepository.UpdateAsync(model);
            int result = await _cateRepository.UnitOfWork.SaveChangesAsync();
            return result > 0
               ? new ReturnResult(true, null, "状态更新成功")
               : new ReturnResult(false, null, "状态更新失败");
        }
    }
}