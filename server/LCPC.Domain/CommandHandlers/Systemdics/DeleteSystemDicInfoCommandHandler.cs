namespace LCPC.Domain.CommandHandlers;

public class DeleteSystemDicInfoCommandHandler:IRequestHandler<DeleteSystemDicInfoCommand,ReturnResult>
{
    
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    private readonly UserHelper _userHelper;
    public DeleteSystemDicInfoCommandHandler(ISystemDicInfoRepository systemDicInfoRepository,UserHelper userHelper)
    {
        _systemDicInfoRepository = systemDicInfoRepository;
        _userHelper = userHelper;
    }
    public async Task<ReturnResult> Handle(DeleteSystemDicInfoCommand request, CancellationToken cancellationToken)
    {
        var list = await _systemDicInfoRepository.GetEntitiesAsync(d => request.Ids.Contains(d.Id));
        int count = list.Count;
        await _systemDicInfoRepository.RemoveAsync(list);

        int result = await _systemDicInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, MessageHelper.DeleteMessage(count))
            : new ReturnResult(false, null, MessageHelper.DeleteMessage(count, false));
    }
}