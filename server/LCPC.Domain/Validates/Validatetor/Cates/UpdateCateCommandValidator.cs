using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class UpdateCateCommandValidator:AbstractValidator<UpdateCateCommand>
    {
        public UpdateCateCommandValidator() {
            RuleFor(x => x.Id).NotEmpty().WithMessage("分类主键不能为空");
            RuleFor(x => x.Name).NotEmpty().WithMessage("分类名称不能为空");
        }
    }
}