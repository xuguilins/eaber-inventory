using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.CommandHandlers
{
    public class DeleteRuleCommandHandler : IRequestHandler<DeleteRuleCommand, ReturnResult>
    {
     private readonly IRuleInfoRepository _ruleRepository;
        public DeleteRuleCommandHandler(IRuleInfoRepository ruleInfoRepository)
        {
            _ruleRepository = ruleInfoRepository;
        }
        public async Task<ReturnResult> Handle(DeleteRuleCommand request, CancellationToken cancellationToken)
        {
           var list= await _ruleRepository.GetEntitiesAsync(x=> request.Ids.Any(v=>v == x.Id));
           int count = list.Count;
           await _ruleRepository.RemoveAsync(list);
           int result = await _ruleRepository.UnitOfWork.SaveChangesAsync();
           return result>0 ? new ReturnResult(true,null,MessageHelper.DeleteMessage(count)) : new ReturnResult(false,null,MessageHelper.DeleteMessage(count,false));
           
        }
    }
}