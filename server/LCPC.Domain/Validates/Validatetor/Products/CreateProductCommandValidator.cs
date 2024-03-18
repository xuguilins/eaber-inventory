namespace LCPC.Domain.Validates.Validatetor;

public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().NotNull().WithMessage("请填写产品名称");

        RuleFor(x => x.UnitId)
            .NotEmpty().NotNull().WithMessage("请选择产品单位");
        RuleFor(x => x.CateId)
            .NotEmpty().NotNull().WithMessage("请选择产品分类");
        RuleFor(x => x.SupilerId)
            .NotEmpty().NotNull().WithMessage("请选择所属供应商");
        RuleFor(x => x.InventoryCount)
            .GreaterThanOrEqualTo(0).WithMessage("库存数量必须大于或等于0");
        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0).WithMessage("最小库存必须或等于0");
        RuleFor(x => x.MaxStock)
            .GreaterThanOrEqualTo(0).WithMessage("最大库存必须或等于0");
        RuleFor(x => x.Purchase)
            .Must((command, arg2) => decimal.TryParse(command.Purchase.ToString(), out arg2))
            .WithMessage("进货价金额不符合规范");
        RuleFor(x => x.InitialCost)
            .Must((command, arg2) => decimal.TryParse(command.Purchase.ToString(), out arg2))
            .WithMessage("初期成本不符合规范");
        RuleFor(x => x.Wholesale)
            .Must((command, arg2) => decimal.TryParse(command.Purchase.ToString(), out arg2))
            .WithMessage("批发价不符合规范");
        RuleFor(x => x.SellPrice)
            .Must((command, arg2) => decimal.TryParse(command.Purchase.ToString(), out arg2))
            .WithMessage("售价金额不符合规范");
        RuleFor(x =>x.SellPrice)
            .GreaterThanOrEqualTo(d=>(d.Wholesale+d.InitialCost))
            .WithMessage("售价金额必须大于【批发价+初期成本】");
        

    }
}