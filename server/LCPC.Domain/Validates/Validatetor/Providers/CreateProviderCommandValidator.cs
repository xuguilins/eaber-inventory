using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class CreateProviderCommandValidator:AbstractValidator<CreateProviderCommand>
    {
        public CreateProviderCommandValidator() {
            RuleFor(x=>x.SupileName).NotNull()
            .NotEmpty().WithMessage("供应商名称不能为空");
            RuleFor(x=>x.TelONE).NotNull().NotEmpty()
            .WithMessage("必须填写一个联系方式");
            RuleFor(x=>x.UserONE).NotNull().NotEmpty()
            .WithMessage("必须填写一个联系人");
           RuleFor(x=>x.Address).NotNull().NotEmpty().WithMessage("联系地址不能为空");      
        }
    }
}