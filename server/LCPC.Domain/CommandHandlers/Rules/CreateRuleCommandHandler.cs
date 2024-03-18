using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class CreateRuleCommandHandler : IRequestHandler<CreateRuleCommand, ReturnResult>
    {
        private readonly IRuleInfoRepository _ruleRepository;
        private readonly UserHelper _userHelper;
        public CreateRuleCommandHandler(IRuleInfoRepository ruleInfoRepository,UserHelper userHelper)
        {
            _ruleRepository = ruleInfoRepository;
            _userHelper = userHelper;
        }
        public async Task<ReturnResult> Handle(CreateRuleCommand request, CancellationToken cancellationToken)
        {
            var model = await _ruleRepository.FindEntity(x => x.RuleName.Equals(request.Name) 
                                                              && x.CreateUser.Equals(_userHelper.LoginName));
            if (model != null)
                throw new Exception("相同类型或不同类型下都不允许出现相同的规则名称");
            var ruleType  = await _ruleRepository.FindEntity(x=>x.RuleType == request.RuleType 
                                                                && x.CreateUser.Equals(_userHelper.LoginName));
            if (ruleType != null)
                throw new Exception("相同编码类型只允许创建一条");
            RuleInfo rule = new RuleInfo
            {
                Enable = request.Enable,
                Formatter = request.RuleFormatter,
                IdentityNum = request.IdentityNum,
                Remark = request.Remark,
                NowValue = 0,
                RuleName = request.Name,
                RuleAppend = request.AppendNum,
                RulePix = request.RulePix,
                RuleType = request.RuleType,
                CreateUser = _userHelper.LoginName
            };
            await  _ruleRepository.AddAsync(rule);
            int result = await _ruleRepository.UnitOfWork.SaveChangesAsync();
            return result>0 ? new ReturnResult(true,null,"创建规则成功")
              : new ReturnResult(false,null,"创建规则失败");
        }
    }
}