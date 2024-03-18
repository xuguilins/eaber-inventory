namespace LCPC.Domain.Validates.Validatetor;

public class OrderConfirmCommandValidator:AbstractValidator<OrderConfirmCommand>
{
    public OrderConfirmCommandValidator()
    {
        RuleFor(x => x.Ids)
            .Must(v => v.Any())
            .WithMessage("选择要确认的订单");
    }
}