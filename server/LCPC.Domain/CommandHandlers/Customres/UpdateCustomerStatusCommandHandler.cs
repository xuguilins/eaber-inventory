namespace LCPC.Domain.CommandHandlers;

public class UpdateCustomerStatusCommandHandler:IRequestHandler<UpdateCustomerStatusCommand,ReturnResult>
{
    private readonly ICustomerInfoRepository _customerInfoRepository;
    public UpdateCustomerStatusCommandHandler(ICustomerInfoRepository customerInfoRepository)
    {
        _customerInfoRepository = customerInfoRepository;
    }
    public async Task<ReturnResult> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
    {
        var model = await _customerInfoRepository.FindEntity(d=>d.Id.Equals(request.Id));
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Enable = !model.Enable;
        await _customerInfoRepository.UpdateAsync(model);
        int result = await _customerInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");
    }
}