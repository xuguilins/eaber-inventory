using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, ReturnResult>
    {
         private readonly ISupilerInfoRepository _supilerInfoRepository;
        public UpdateProviderCommandHandler(ISupilerInfoRepository supilerInfoRepository) {
            _supilerInfoRepository = supilerInfoRepository;
        }
        public  async Task<ReturnResult> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
        {
             var model =await _supilerInfoRepository.GetByKey(request.Id);
             if (model == null) 
                 throw new Exception("未找到到有效的供应商");
            model.Enable = request.Enable;
            model.SupTel = request.TelONE;
            model.SupTelT = request.TelTWO;
            model.Address = request.Address;
            model.ProviderUser = request.UserONE;
            model.ProviderUserT = request.UserTWO;
            model.SupPhone = request.PhoneONE;
            model.SupPhoneT = request.PhoneTWO;
            model.Remark  = request.Remark;
            model.SupName = request.SupileName;
           await _supilerInfoRepository.UpdateAsync(model);
             int result = await _supilerInfoRepository.UnitOfWork.SaveChangesAsync();
            return result>0? new ReturnResult(true,null,"供应商更新成功")
            :new ReturnResult(false,null,"供应商更新失败");
        }
    }
}