namespace LCPC.Domain.CommandHandlers;

public class CreateExtraCommandHandler:IRequestHandler<CreateExtraCommand,ReturnResult>
{
    private readonly IExtraOrderRepository _extraOrderRepository;
    private readonly UserHelper _userHelper;
    private readonly IRuleManager _ruleManager;
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    public CreateExtraCommandHandler(IExtraOrderRepository extraOrderRepository,
        UserHelper userHelper,
        IRuleManager ruleManager,
        ISystemDicInfoRepository systemDicInfoRepository)
    {
        _userHelper = userHelper;
        _ruleManager = ruleManager;
        _systemDicInfoRepository = systemDicInfoRepository;
        _extraOrderRepository = extraOrderRepository;
    }
    public async Task<ReturnResult> Handle(CreateExtraCommand request, CancellationToken cancellationToken)
    {
        var user = _userHelper.LoginName;
        var types = await _systemDicInfoRepository.GetEntitiesAsync(d => d.DicType == DicType.OthersIN || d.DicType == DicType.OthersOut);
        var typeModel = types.FirstOrDefault(d => d.DicCode == request.TypeName && d.Enable);
        if (typeModel == null)
            throw new Exception("未找到有效的类型或类型已被禁用");
        string code =  await _ruleManager.getNextRuleNumber(RuleType.ExtraOrder);
        ExtraOrder order = new ExtraOrder();
        order.CreateUser = user;
        order.OrderCode = code;
        order.ExtraType = request.ExtraType;
        order.TypeName = typeModel.DicCode;
        order.Remark = request.Remark;
        order.Price = request.Price;
        order.Enable = true;
        await _extraOrderRepository.AddAsync(order);
        int reslt = await _extraOrderRepository.UnitOfWork.SaveChangesAsync();
        return reslt > 0
            ? new ReturnResult(true, null, "新增成功")
            : new ReturnResult(false, null, "新增失败");

    }
}