using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace LCPC.Domain.Validates.Validatetor
{
    public class UpdateUserCommandValidator:AbstractValidator<UpdateUserCommand>
    {
          public UpdateUserCommandValidator(){
                RuleFor(x=>x.Id).NotEmpty().WithMessage("用户Id不能为空");
                RuleFor(x => x.Name).NotEmpty().WithMessage("用户名不能为空");
                RuleFor(x=>x.Pass).NotEmpty().WithMessage("密码不能为空");
          }
    }
}