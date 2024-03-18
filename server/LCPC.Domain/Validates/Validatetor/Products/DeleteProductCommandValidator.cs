namespace LCPC.Domain.Validates.Validatetor;

public class DeleteProductCommandValidator:AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Ids)
            .Must(v => v.Any())
            .WithMessage("请选择要删除的数据");
    }
}