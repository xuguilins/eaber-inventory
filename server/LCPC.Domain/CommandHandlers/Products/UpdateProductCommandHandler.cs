using TinyPinyin;

namespace LCPC.Domain.CommandHandlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IRuleManager _ruleManager;
    private readonly ICatetoryRepository _catetoryRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    public UpdateProductCommandHandler(IProdcutRepository prodcutRepository,
        IRuleManager ruleManager,
        ICatetoryRepository catetoryRepository,
        ISupilerInfoRepository supilerInfoRepository,
        ISystemDicInfoRepository systemDicInfoRepository
    )
    {
        _prodcutRepository = prodcutRepository;
        _ruleManager = ruleManager;
        _systemDicInfoRepository = systemDicInfoRepository;
        _catetoryRepository = catetoryRepository;
        _supilerInfoRepository = supilerInfoRepository;
    }

    public async Task<ReturnResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var model = await _prodcutRepository.GetByKey(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
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
        model.SellPrice = request.SellPrice;
        model.CateId = request.CateId;
        model.UnitId = request.UnitId;
        model.SupilerId = request.SupilerId;
        model.ProductName = request.ProductName;
        model.NameSpell = PinyinHelper.GetPinyinInitials(model.ProductName);
        model.ProductModel = request.ProductModel;
        model.Purchase = request.Purchase;
        model.ConversionRate = request.ConversionRate;
        model.Wholesale = request.Wholesale;
        model.InitialCost = request.InitialCost;
        model.InventoryCount = request.InventoryCount;
        model.MinStock = request.MinStock;
        model.MaxStock = request.MaxStock;
        model.Remark = request.Remark;
        model.Enable = request.Enable;
        await _prodcutRepository.UpdateAsync(model);
        int result = await _prodcutRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "商品更新成功")
            : new ReturnResult(false, null, "商品更新失败");
    }
}