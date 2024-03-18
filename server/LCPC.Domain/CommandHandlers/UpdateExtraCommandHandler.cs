namespace LCPC.Domain.CommandHandlers;

public class UpdateExtraCommandHandler:IRequestHandler<UpdateExtraCommand,ReturnResult>
{
    private readonly IExtraOrderRepository _extraOrderRepository;
    public UpdateExtraCommandHandler(IExtraOrderRepository extraOrderRepository)
    {
        _extraOrderRepository = extraOrderRepository;
    }
    public async Task<ReturnResult> Handle(UpdateExtraCommand request, CancellationToken cancellationToken)
    {
        var model = await _extraOrderRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Remark = request.Remark;
        model.Price = request.Price;
        model.ExtraType = request.ExtraType;
        model.TypeName = request.TypeName;
        await _extraOrderRepository.UpdateAsync(model);
        int result = await _extraOrderRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");
    }
}