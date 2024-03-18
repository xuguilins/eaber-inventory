namespace LCPC.Domain.CommandHandlers;

public class UpdateSystemDicInfoStatusCommandHandler:IRequestHandler<UpdateSystemDicInfoStatusCommand,ReturnResult>
{
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    private readonly UserHelper _userHelper;
    public UpdateSystemDicInfoStatusCommandHandler(ISystemDicInfoRepository systemDicInfoRepository,UserHelper userHelper)
    {
        _systemDicInfoRepository = systemDicInfoRepository;
        _userHelper = userHelper;
    }

    public async Task<ReturnResult> Handle(UpdateSystemDicInfoStatusCommand request, CancellationToken cancellationToken)
    {
        var model = await _systemDicInfoRepository.FindEntity(d => d.Id.Equals(request.Id));
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.Enable = !model.Enable;
        await _systemDicInfoRepository.UpdateAsync(model);
        int result = await _systemDicInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");
    }
}