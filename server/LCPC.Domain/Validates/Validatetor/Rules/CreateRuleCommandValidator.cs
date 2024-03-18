using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class CreateRuleCommandValidator:AbstractValidator<CreateRuleCommand>
    {
        public CreateRuleCommandValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("规则名称不能为空");
            RuleFor(x=>x.IdentityNum).GreaterThan(0).WithMessage("自赠数必须大于0");
            RuleFor(x=>x.AppendNum).GreaterThan(0).WithMessage("补位数必须大于0");
            RuleFor(x=>x.RuleFormatter).NotEmpty().WithMessage("规则格式不能为空");
        }
    }
}