using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCPC.Domain.Commands.Providers;

namespace LCPC.Domain.CommandHandlers.Providers
{
    public class ProviderExcelCommandHandler : IRequestHandler<ProviderExcelCommand<SupilerExcelDto>, ReturnResult>
    {
        private readonly ISupilerInfoRepository _supilerInfoRepository;
        private readonly IRuleManager _ruleManager;
        public ProviderExcelCommandHandler(ISupilerInfoRepository supilerInfoRepository, IRuleManager ruleManager)
        {
            _supilerInfoRepository = supilerInfoRepository;
            _ruleManager = ruleManager;
        }
        public async Task<ReturnResult> Handle(ProviderExcelCommand<SupilerExcelDto> request, CancellationToken cancellationToken)
        {
            if (!request.Providers.Any())
                throw new Exception("导入的数据无效，请检查导入文件");
            // 获取现有数据
            var supilerInfos = _supilerInfoRepository.GetEntities.ToList();

            List<SupplierInfo> suppliers = new List<SupplierInfo>();
            foreach (var item in request.Providers)
            {
                var model = supilerInfos.FirstOrDefault(x => x.SupName.Equals(item.SupileName));
                if (model == null && !string.IsNullOrWhiteSpace(item.SupileName))
                {
                    string nummber = await _ruleManager.getNextRuleNumber(RuleType.supplier);
                    suppliers.Add(new SupplierInfo
                    {
                        SupName = item.SupileName,
                        Address = item.Address,
                        Enable = true,
                        ProviderUser = item.UserONE,
                        ProviderUserT = item.UserTWO,
                        SupPhone = item.PhoneONE,
                        SupPhoneT = item.PhoneTWO,
                        SupTel = item.TelONE,
                        SupTelT = item.TelTWO,
                        SupNumber = nummber,
                        Remark = item.Remark
                    });
                }

            }
            await _supilerInfoRepository.AddRangeAsync(suppliers);
            int result = await _supilerInfoRepository.UnitOfWork.SaveChangesAsync();
            return result >= 0 ? new ReturnResult(true, null, "成功导入【" + suppliers.Count + "】条数据")
             : new ReturnResult(false, null, "导入失败");
        }
    }
}