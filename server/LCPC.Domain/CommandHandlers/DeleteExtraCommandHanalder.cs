namespace LCPC.Domain.CommandHandlers;

public class DeleteExtraCommandHanalder:IRequestHandler<DeleteExtraCommand,ReturnResult>
{
    private readonly IExtraOrderRepository _extraOrderRepository;
    public DeleteExtraCommandHanalder(IExtraOrderRepository extraOrderRepository)
    {
        _extraOrderRepository = extraOrderRepository;
    }
    public async Task<ReturnResult> Handle(DeleteExtraCommand request, CancellationToken cancellationToken)
    {
        var list = await _extraOrderRepository.GetEntitiesAsync(d => request.Ids.Contains(d.Id));
        int count = list.Count;
        await _extraOrderRepository.RemoveAsync(list);
        int result = await _extraOrderRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, MessageHelper.DeleteMessage(count))
            : new ReturnResult(false, null, MessageHelper.DeleteMessage(count, false));
    }
}