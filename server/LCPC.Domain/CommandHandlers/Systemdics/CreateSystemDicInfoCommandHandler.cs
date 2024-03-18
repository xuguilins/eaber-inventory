namespace LCPC.Domain.CommandHandlers;

public class CreateSystemDicInfoCommandHandler:IRequestHandler<CreateSystemDicInfoCommand,ReturnResult>
{
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    private readonly UserHelper _userHelper;
    public CreateSystemDicInfoCommandHandler(ISystemDicInfoRepository systemDicInfoRepository,UserHelper userHelper)
    {
        _systemDicInfoRepository = systemDicInfoRepository;
        _userHelper = userHelper;
    }
    public async Task<ReturnResult> Handle(CreateSystemDicInfoCommand request, CancellationToken cancellationToken)
    {
        var user = _userHelper.LoginName;
        var model = await _systemDicInfoRepository.FindEntity(
            d => d.DicType == request.DicType
                 && d.CreateUser.Equals(user)
                 && d.DicName.Equals(request.DicName));
        if (model != null)
            throw new Exception("已经存在相同的字典");
        if (string.IsNullOrWhiteSpace(request.DicCode))
            request.DicCode = request.DicName;

        SystemDicInfo dicInfo = new SystemDicInfo
        {
            CreateUser = user,
            Enable = request.Enable,
            Remark = request.Remark,
            DicCode = request.DicCode,
            DicName = request.DicName,
            DicType = request.DicType
        };
        await _systemDicInfoRepository.AddAsync(dicInfo);
        int result = await _systemDicInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "创建成功")
            : new ReturnResult(false, null, "创建失败");
    }
}