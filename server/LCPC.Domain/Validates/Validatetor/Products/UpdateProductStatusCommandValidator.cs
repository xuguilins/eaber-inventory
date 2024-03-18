namespace LCPC.Domain.Validates.Validatetor;

public class UpdateProductStatusCommandValidator:AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().NotNull()
            .WithMessage("数据主键不能为空");
    }
}