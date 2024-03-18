using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace LCPC.Domain.Validates.Validatetor
{
    public class UserLoginCommandValidator:AbstractValidator<UserLoginCommand>
    {
         public UserLoginCommandValidator(){
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.UserPass).NotEmpty().WithMessage("密码不能为空");
         }
    }
}