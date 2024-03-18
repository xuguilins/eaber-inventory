namespace LCPC.Domain.Validates.Validatetor;

public class CreateCustomerCommandValidator:AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(d => d.CustomerName)
            .NotEmpty()
            .NotNull().WithMessage("客户名称不能为空");
    }
}