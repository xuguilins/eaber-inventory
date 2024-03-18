namespace LCPC.Domain.CommandHandlers;

public class UpdatePuraseStatusHandler:IRequestHandler<UpdatePuraseStatusCommand,ReturnResult>
{
    private readonly IPurchaseInRepository _purchaseInRepository;
    public UpdatePuraseStatusHandler(IPurchaseInRepository purchaseInRepository)
    {
        _purchaseInRepository = purchaseInRepository;
    }
    public async Task<ReturnResult> Handle(UpdatePuraseStatusCommand request, CancellationToken cancellationToken)
    {
        var model = await _purchaseInRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.InOStatus = request.InOStatus;
        await _purchaseInRepository.UpdateAsync(model);
        int result = await _purchaseInRepository.UnitOfWork.SaveChangesAsync();
        return result > 0 ? new ReturnResult(true, null, "操作成功") : new ReturnResult(false, null, "操作失败");
    }
}