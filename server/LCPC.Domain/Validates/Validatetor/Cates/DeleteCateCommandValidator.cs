using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class DeleteCateCommandValidator:AbstractValidator<DeleteCateCommand>
    {
        public DeleteCateCommandValidator() {
            RuleFor(x=>x.Ids).Must(v=>v.Any()).WithMessage
            ("删除的数据主键不能为空");
        }
    }
}