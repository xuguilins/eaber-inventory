using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateCateCommandHandler : IRequestHandler<UpdateCateCommand, ReturnResult>
    {
        private readonly ICatetoryRepository _cateRepository;
        public UpdateCateCommandHandler(ICatetoryRepository catetoryRepository)
        {
            _cateRepository = catetoryRepository;
        }
        public async Task<ReturnResult> Handle(UpdateCateCommand request, CancellationToken cancellationToken)
        {
            if(request.ParentId == request.Id)
                throw new Exception("自身不能用作自身的上级");
           // _cateRepository.Handler += Test;
            var data = await _cateRepository.GetByKey(request.Id);
           
            if (data == null)
                throw new Exception("无效的分类数据");
            data.ParentId = request.ParentId;
            data.CateName = request.Name;
            data.Enable = request.Enable;
            data.Remark = request.Remark;
            await _cateRepository.UpdateAsync(data);
            int result = await _cateRepository.UnitOfWork.SaveChangesAsync();
            return result > 0 ? new ReturnResult(true, null, "分类更新成功") : new ReturnResult(false, null, "分类更新失败");
        }

        private void Test(object send, CateInfo data)
        {
            
        }
    }
}