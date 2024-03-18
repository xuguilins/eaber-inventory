using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class DeleteRuleCommandValidator:AbstractValidator<DeleteRuleCommand>
    {
         public  DeleteRuleCommandValidator() {
            RuleFor(x=>x.Ids).Must(x=>x.Count()>0).WithMessage("请选择要删除的规则");
         }
    }
}