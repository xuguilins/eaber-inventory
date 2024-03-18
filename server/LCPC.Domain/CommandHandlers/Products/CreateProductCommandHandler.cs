using TinyPinyin;

namespace LCPC.Domain.CommandHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IRuleManager _ruleManager;
    private readonly ICatetoryRepository _catetoryRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly UserHelper _userHelper;
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    public CreateProductCommandHandler(IProdcutRepository prodcutRepository,
        IRuleManager ruleManager,

        ICatetoryRepository catetoryRepository,
        ISupilerInfoRepository supilerInfoRepository,
        ISystemDicInfoRepository systemDicInfoRepository,
        UserHelper userHelper 
    )
    {
        _prodcutRepository = prodcutRepository;
        _ruleManager = ruleManager;
        _systemDicInfoRepository = systemDicInfoRepository;
        _catetoryRepository = catetoryRepository;
        _supilerInfoRepository = supilerInfoRepository;
        _userHelper = userHelper;
    }

    public async Task<ReturnResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var model = await _prodcutRepository.FindEntity(x => x.ProductName.Equals(request.ProductName)
                                                             && x.ProductModel.Equals(request.ProductModel)
                                                             && x.CreateUser.Equals(_userHelper.LoginName));
        if (model != null)
            throw new Exception("已存在同名称同型号的产品");
        // 验证单位有效性
         var unit = await _systemDicInfoRepository.GetByKey(request.UnitId);
        if (unit == null || (unit != null && !unit.Enable))
            throw new Exception("无效的数据单位或当前单位已被禁用");
        var cate = await _catetoryRepository.GetByKey(request.CateId);
        if (cate == null || (cate != null && !cate.Enable))
            throw new Exception("无效的数据分类或当前分类已被禁用");
        var supiler = await _supilerInfoRepository.GetByKey(request.SupilerId);
        if (supiler == null || (supiler != null && !supiler.Enable))
            throw new Exception("无效的供应商或供应商已被禁用");
        ;
        string number = await _ruleManager.getNextRuleNumber(RuleType.Product);
        var product = new ProductInfo
        {
            ProductCode = number,
            SellPrice = request.SellPrice,
            CateId = request.CateId,
            UnitId = request.UnitId,
            SupilerId = request.SupilerId,
            ProductName = request.ProductName,
            ProductModel = request.ProductModel,
            Purchase = request.Purchase,
            ConversionRate = request.ConversionRate,
            Wholesale = request.Wholesale,
            InitialCost = request.InitialCost,
            InventoryCount = request.InventoryCount,
            MinStock = request.MinStock,
            MaxStock = request.MaxStock,
            Remark = request.Remark,
            Enable = request.Enable,
            CreateUser = _userHelper.LoginName,
            NameSpell = PinyinHelper.GetPinyinInitials(request.ProductName)
        };
        await _prodcutRepository.AddAsync(product);
        int result = await _prodcutRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "商品录入成功")
            : new ReturnResult(false, null, "商品录入失败");
    }
}