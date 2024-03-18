using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class UpdateRuleStatusCommandHandler : IRequestHandler<UpdateRuleStatusCommand, ReturnResult>
    {
          private readonly IRuleInfoRepository _ruleRepository;
        public UpdateRuleStatusCommandHandler(IRuleInfoRepository ruleInfoRepository)
        {
            _ruleRepository = ruleInfoRepository;
        }
        public async Task<ReturnResult> Handle(UpdateRuleStatusCommand request, CancellationToken cancellationToken)
        {
             
            var model = await _ruleRepository.FindEntity(x => x.Id == request.Id);
            if (model == null)
                throw new Exception("未找到有效的数据");
            model.Enable = !model.Enable;
            await _ruleRepository.UpdateAsync(model);
            int result = await _ruleRepository.UnitOfWork.SaveChangesAsync();
            return result > 0
               ? new ReturnResult(true, null, "状态更新成功")
               : new ReturnResult(false, null, "状态更新失败");
        }
    }
}