using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace LCPC.Domain.Validates.Validatetor
{
    public class DeleteUserCommandValidator:AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator() {
            RuleFor(x=>x.Ids).Must(x=>x.Length>0).WithMessage("无效的数据id");
        }
    }
}