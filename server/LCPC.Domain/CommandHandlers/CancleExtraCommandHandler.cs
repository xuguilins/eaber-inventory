namespace LCPC.Domain.CommandHandlers;

public class CancleExtraCommandHandler:IRequestHandler<CancleExtraCommand,ReturnResult>
{
    private readonly IExtraOrderRepository _extraOrderRepository;
    public CancleExtraCommandHandler(IExtraOrderRepository extraOrderRepository)
    {
        _extraOrderRepository = extraOrderRepository;
    }
    public async Task<ReturnResult> Handle(CancleExtraCommand request, CancellationToken cancellationToken)
    {
        var model = await _extraOrderRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Enable = !model.Enable;
        await _extraOrderRepository.UpdateAsync(model);
        int result = await _extraOrderRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");
    }
}