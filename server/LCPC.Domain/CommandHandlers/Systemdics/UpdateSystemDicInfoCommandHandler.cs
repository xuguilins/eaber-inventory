namespace LCPC.Domain.CommandHandlers;

public class UpdateSystemDicInfoCommandHandler:IRequestHandler<UpdateSystemDicInfoCommand,ReturnResult>
{
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    private readonly UserHelper _userHelper;
    public UpdateSystemDicInfoCommandHandler(ISystemDicInfoRepository systemDicInfoRepository,UserHelper userHelper)
    {
        _systemDicInfoRepository = systemDicInfoRepository;
        _userHelper = userHelper;
    }

    public async Task<ReturnResult> Handle(UpdateSystemDicInfoCommand request, CancellationToken cancellationToken)
    {
        var model = await _systemDicInfoRepository.FindEntity(d => d.Id.Equals(request.Id));
        if (model == null)
            throw new Exception("未找到有效的数据");
        model.DicCode = request.DicCode;
        model.DicName = request.DicName;
        model.Enable = request.Enable;
        model.Remark = request.Remark;
        await _systemDicInfoRepository.UpdateAsync(model);
        int result = await _systemDicInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "更新成功")
            : new ReturnResult(false, null, "更新失败");

    }
}