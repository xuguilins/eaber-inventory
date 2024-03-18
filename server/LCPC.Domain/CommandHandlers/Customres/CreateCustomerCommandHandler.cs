using TinyPinyin;

namespace LCPC.Domain.CommandHandlers;

public class CreateCustomerCommandHandler:IRequestHandler<CreateCustomerCommand,ReturnResult>
{
    private readonly ICustomerInfoRepository _customerInfoRepository;
    private readonly UserHelper _userHelper;
    private readonly IRuleManager _ruleManager;
    public CreateCustomerCommandHandler(ICustomerInfoRepository customerInfoRepository,UserHelper userHelper,IRuleManager ruleManager)
    {
        _customerInfoRepository = customerInfoRepository;
        _userHelper = userHelper;
        _ruleManager = ruleManager;
    }
    public async Task<ReturnResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var model = await _customerInfoRepository.FindEntity(d =>
            d.CreateUser.Equals(_userHelper.LoginName)
            && d.CustomerName.Equals(request.CustomerName));
        if (model != null)
            throw new Exception("已存在相同的客户名称");
        var code = await _ruleManager.getNextRuleNumber(RuleType.Customer);
        CustomerInfo info = new CustomerInfo
        {
            TelNumber = request.TelNumber,
            PhoneNumber = request.PhoneNumber,
            CustomerCode = code,
            CustomerName = request.CustomerName,
            Remark = request.Remark,
            CreateUser = _userHelper.LoginName,
            Address = request.Address,
            CustomerUser = request.CustomerUser,
            Enable = request.Enable,
            NameSpell = PinyinHelper.GetPinyinInitials(request.CustomerName)
        };
        await _customerInfoRepository.AddAsync(info);
        int result = await _customerInfoRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "创建成功")
            : new ReturnResult(false, null, "创建失败");
    }
}