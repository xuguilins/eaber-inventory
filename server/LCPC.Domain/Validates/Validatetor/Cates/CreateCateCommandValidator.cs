using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace LCPC.Domain.Validates.Validatetor
{
    public class CreateCateCommandValidator:AbstractValidator<CreateCateCommand>
    {
         public CreateCateCommandValidator() {
            RuleFor(x => x.Name).NotEmpty().WithMessage("分类名称不能为空");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("分类名称不能超过100个字符");
        
         }
    }
}