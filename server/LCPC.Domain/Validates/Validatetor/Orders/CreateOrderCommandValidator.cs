using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.Validates.Validatetor
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.SellUser).NotEmpty().NotNull().WithMessage("购买单位不能为空");
            RuleFor(x => x.SellTime).NotEmpty().NotNull().WithMessage("单据时间不能为空");
            RuleFor(x => x.SellPhone).NotEmpty().NotNull().WithMessage("联系方式不能为空");
         
            RuleFor(x=>x.Products)
            .Must(v=>v.Any()).WithMessage("请选择产品");
        }
    }
}