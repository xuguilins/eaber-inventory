namespace LCPC.Domain.CommandHandlers;

public class UpdateProductStatusCommandHandler:IRequestHandler<UpdateProductStatusCommand,ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    public UpdateProductStatusCommandHandler(IProdcutRepository prodcutRepository)
    {
        _prodcutRepository = prodcutRepository;
    }
    public async Task<ReturnResult> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var model = await _prodcutRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Enable = !model.Enable;
        await _prodcutRepository.UpdateAsync(model);
        int result = await _prodcutRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "状态更新成功")
            : new ReturnResult(false, null, "状态更新失败");

    }
}