namespace LCPC.Domain.CommandHandlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    public DeleteProductCommandHandler(IProdcutRepository prodcutRepository)
    {
        _prodcutRepository = prodcutRepository;
    }
    public async Task<ReturnResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var list = await _prodcutRepository.GetEntitiesAsync(x => request.Ids.Contains(x.Id));
        await _prodcutRepository.RemoveAsync(list);
        int result = await _prodcutRepository.UnitOfWork.SaveChangesAsync();
        return result>0? new ReturnResult(true,null,MessageHelper.DeleteMessage(list.Count))
            :new ReturnResult(false,null,MessageHelper.DeleteMessage(list.Count,false));
    }
}