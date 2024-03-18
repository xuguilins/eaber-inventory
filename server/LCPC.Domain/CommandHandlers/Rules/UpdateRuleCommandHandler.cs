using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateRuleCommandHandler : IRequestHandler<UpdateRuleCommand, ReturnResult>
    {
         private readonly IRuleInfoRepository _ruleRepository;
        public UpdateRuleCommandHandler(IRuleInfoRepository ruleInfoRepository)
        {
            _ruleRepository = ruleInfoRepository;
        }
        public async Task<ReturnResult> Handle(UpdateRuleCommand request, CancellationToken cancellationToken)
        {
            var model = await _ruleRepository.GetByKey(request.Id);
            if (model == null)
               throw new Exception("未找到有效的数据");
            model.Enable = request.Enable;
            model.RuleAppend = request.AppendNum;
            model.RulePix =request.RulePix;
            model.RuleName = request.Name;
            model.Formatter=request.RuleFormatter;
            model.IdentityNum  = request.IdentityNum;
            model.Remark =request.Remark;
            await _ruleRepository.UpdateAsync(model);
            int result = await _ruleRepository.UnitOfWork.SaveChangesAsync();
            return result>0 ? new ReturnResult(true,null,"更新规则成功")
             : new ReturnResult(false,null,"更新规则失败");
        }
    }
}