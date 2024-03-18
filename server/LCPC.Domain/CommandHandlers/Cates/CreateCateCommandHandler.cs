using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class CreateCateCommandHandler : IRequestHandler<CreateCateCommand, ReturnResult>
    {
        private readonly ICatetoryRepository _cateRepository;
        private readonly UserHelper _userHelper;
        public CreateCateCommandHandler(ICatetoryRepository catetoryRepository,UserHelper userHelper)
        {
            _cateRepository = catetoryRepository;
            _userHelper = userHelper;
        }
        public async Task<ReturnResult> Handle(CreateCateCommand request, CancellationToken cancellationToken)
        {
            var model = await _cateRepository
                .FindEntity(x => x.CateName == request.Name && x.CreateUser.Equals(_userHelper.LoginName));
            if (model != null && model.Enable)
                throw new Exception("分类名称已存在");
            var cate = new CateInfo
            {
                CateName = request.Name,
                Enable = request.Enable,
                Remark = request.Remark,
                ParentId = request.ParentId,
                CreateUser = _userHelper.LoginName
           
            };
            await _cateRepository.AddAsync(cate);
            int result = await  _cateRepository.UnitOfWork.SaveChangesAsync();
            return result>0? new ReturnResult(true,null,"分类创建成功") : new ReturnResult(false,null,"分类创建失败");
                  
        }
    }
}