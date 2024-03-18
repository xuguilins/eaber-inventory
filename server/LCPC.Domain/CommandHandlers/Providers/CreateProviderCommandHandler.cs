using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LCPC.Domain.Commands;
using LCPC.Domain.Entities;
using LCPC.Domain.IRepositories;
using LCPC.Domain.IServices;
using LCPC.Share.Response;
using MediatR;

namespace LCPC.Domain.CommandHandlers
{
    public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, ReturnResult>
    {
        private readonly ISupilerInfoRepository _supilerInfoRepository;
        private readonly IRuleManager _ruleManager;
        private readonly UserHelper _userHelper;
        public CreateProviderCommandHandler(ISupilerInfoRepository supilerInfoRepository, IRuleManager ruleManager,UserHelper userHelper)
        {
            _supilerInfoRepository = supilerInfoRepository;
            _userHelper = userHelper;
            _ruleManager = ruleManager;
        }
        public async Task<ReturnResult> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
        {
            var model = await _supilerInfoRepository
                .FindEntity(x => x.SupName.Equals(request.SupileName)
                && x.CreateUser.Equals(_userHelper.LoginName));
            if (model != null)
                throw new Exception("已存在相同的供应商");
            var number = await _ruleManager.getNextRuleNumber(RuleType.supplier);
            var supiler = new SupplierInfo
            {
                Address = request.Address,
                Remark = request.Remark,
                Enable = request.Enable,
                ProviderUser = request.UserONE,
                ProviderUserT = request.UserTWO,
                SupName = request.SupileName,
                SupNumber = number,
                SupPhone = request.PhoneONE,
                SupPhoneT = request.PhoneTWO,
                SupTel = request.TelONE,
                SupTelT = request.TelTWO,
                CreateUser = _userHelper.LoginName
            };
            await _supilerInfoRepository.AddAsync(supiler);
            int result = await _supilerInfoRepository.UnitOfWork.SaveChangesAsync();
            return result>0? new ReturnResult(true,null,"供应商创建成功")
             : new ReturnResult(false,null,"供应商创建失败");

        }
    }
}