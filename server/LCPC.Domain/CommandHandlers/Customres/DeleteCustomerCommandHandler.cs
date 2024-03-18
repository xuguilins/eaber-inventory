namespace LCPC.Domain.CommandHandlers;

public class DeleteCustomerCommandHandler:IRequestHandler<DeleteCustomerCommand,ReturnResult>
{
    private readonly ICustomerInfoRepository _customerInfoRepository;
    public DeleteCustomerCommandHandler(ICustomerInfoRepository customerInfoRepository)
    {
        _customerInfoRepository = customerInfoRepository;
    }
    public async Task<ReturnResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var list = await _customerInfoRepository.GetEntitiesAsync(d => request.Ids.Contains(d.Id));
        int count = list.Count;
        await _customerInfoRepository.RemoveAsync(list);
        int result = await _customerInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, MessageHelper.DeleteMessage(count))
            : new ReturnResult(false, null, MessageHelper.DeleteMessage(count, false));
    }
}