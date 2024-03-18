using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace LCPC.Domain.Validates.Validatetor
{
    public class CreateUserCommandValidator:AbstractValidator<CreatUserCommand>
    {
        public CreateUserCommandValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x=>x.Pass).NotEmpty().WithMessage("密码不能为空");
        }
    }
    
}