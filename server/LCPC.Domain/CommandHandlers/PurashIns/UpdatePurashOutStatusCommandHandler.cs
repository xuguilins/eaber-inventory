namespace LCPC.Domain.CommandHandlers;

public class UpdatePurashOutStatusCommandHandler:IRequestHandler<UpdatePurashOutStatusCommand,ReturnResult>
{
    private readonly IPurchaseOutOrderRepository _purchaseOutOrderRepository;
    public UpdatePurashOutStatusCommandHandler(IPurchaseOutOrderRepository purchaseOutOrderRepository)
    {
        _purchaseOutOrderRepository = purchaseOutOrderRepository;
    }
    public async Task<ReturnResult> Handle(UpdatePurashOutStatusCommand request, CancellationToken cancellationToken)
    {
        var model = await _purchaseOutOrderRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的退货单数据");
        model.OutStatus = request.OutStatus;
        await _purchaseOutOrderRepository.UpdateAsync(model);
        int result = await _purchaseOutOrderRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "退货单已确认")
            : new ReturnResult(false, null, "退货单确认失败");
    }
}