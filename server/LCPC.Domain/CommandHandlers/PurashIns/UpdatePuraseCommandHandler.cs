using LCPC.Domain.Queries;

namespace LCPC.Domain.CommandHandlers;

public class UpdatePuraseCommandHandler:IRequestHandler<UpdatePuraseCommand,ReturnResult>
{
    private readonly ISqlDapper _sqlDapper;
    private readonly IProductQueries _productQueries;
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IPurchaseInRepository _purchaseInRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly IPurchaseInDetailRepository _purchaseInDetailRepository;
 
    
    private readonly IRuleManager _ruleManager;
    private readonly UserHelper _userHelper;
    public UpdatePuraseCommandHandler(ISqlDapper sqlDapper,
        IProductQueries productQueries,
        IProdcutRepository prodcutRepository,
        IPurchaseInRepository purchaseInRepository,
        ISupilerInfoRepository supilerInfoRepository,
        IRuleManager ruleManager,
        UserHelper userHelper,
        IPurchaseInDetailRepository purchaseInDetailRepository
    )
    {
        _sqlDapper = sqlDapper;
        _productQueries = productQueries;
        _prodcutRepository = prodcutRepository;
        _purchaseInRepository = purchaseInRepository;
        _ruleManager = ruleManager;
        _supilerInfoRepository = supilerInfoRepository;
        _userHelper = userHelper;
        _purchaseInDetailRepository = purchaseInDetailRepository;
    }
    public async Task<ReturnResult> Handle(UpdatePuraseCommand request, CancellationToken cancellationToken)
    {
        var model = await   _purchaseInRepository.GetPuraseInOrder(request.Id);
        if (model == null)
            throw new Exception("未找到有效的数据");
        // 更新进货单
        model.SupplierId = await CheckSupilerChange(request.SupileName, request);
        model.InPhone = request.InPhone;
        model.Logistics = request.Logistics;
        model.InOrderTime = request.InOrderTime;
        model.ChannelType = request.ChannelType;
        model.InUser = request.InUser;
        model.InPhone = request.InPhone;
        model.Remark = request.Remark;
        // 创建商品
        #region 删除原来的进货单明细/释放商品
        foreach (var product in model.PurchaseInDetails)
        {
            await DisposeProduct(product.ProductId, product.ProductCount);
        }
        await _purchaseInDetailRepository.RemoveAsync(model.PurchaseInDetails);
        #endregion
        //更新商品
        var details = request.PrdocutDetail;
        List<PurchaseInDetail> children = new List<PurchaseInDetail>();
        model.PurchaseInDetails = new List<PurchaseInDetail>();
        foreach (var item in details)
        {
            PurchaseInDetail detail = new PurchaseInDetail();
            detail.ProductCode = item.ProductCode;
            detail.ProductAll = (item.ProductCount * item.ProductPrice);
            detail.ProductModel = item.ProductModel;
            detail.ProductName = item.ProductName;
            detail.ProductCount = item.ProductCount;
            detail.ProductPrice = item.ProductPrice;
            detail.PurchaseInId = model.Id;
            detail.Remark = item.Remark;
            detail.CreateUser = model.CreateUser;
            var product= await _prodcutRepository.GetProductByCode(item.ProductCode);
            if (product != null) 
            {
                // 商品存在
                detail.ProductId = product.Id;
                await UpdateProduct(product, item);
            }
            else
            {
                var code = await _ruleManager.getNextRuleNumber(RuleType.Product);
                detail.ProductId = await CreateProduct(item, model.SupplierId, code);
                detail.ProductCode = code; 
              
            }
            model.PurchaseInDetails.Add(detail);
        }

        model.InCount = model.PurchaseInDetails.Sum(d => d.ProductCount);
        model.InPrice = model.PurchaseInDetails.Sum(d => d.ProductAll);
        await _purchaseInRepository.UpdateAsync(model);
        int result = await _purchaseInRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "采购单更新成功")
            : new ReturnResult(false, null, "采购单更新失败");
    }
    private async Task<string>  CreateProduct(PurashDetailOutDto item,string  SupplierId,string code)
    {
        
        var product = new ProductInfo
        {
            CateId = item.CateId,
            UnitId = item.UnitId,
            ProductName = item.ProductName,
            ProductModel = item.ProductModel,
            ProductCode = code,
            SellPrice = item.SellPrice,
            Wholesale = item.ProductWocost,
            InitialCost = item.ProductIncost,
            Purchase = item.ProductPrice,
            SupilerId = SupplierId,
            InventoryCount = item.ProductCount,
            MinStock = 1,
            MaxStock = 100,
            CreateUser = _userHelper.LoginName
        };
        await _prodcutRepository.AddAsync(product);
        return product.Id;
    }

    private async Task DisposeProduct(string productId, int count)
    {
        var product = await _prodcutRepository.GetByKey(productId);
        if (product != null)
            product.InventoryCount -= count;
        await _prodcutRepository.UpdateAsync(product);
    }

    private async Task UpdateProduct(ProductInfo model,PurashDetailDto item)
    {
        model.Wholesale = item.ProductWocost;
        model.SellPrice = item.SellPrice;
        model.InitialCost = item.ProductIncost;
        model.Purchase = item.ProductPrice;
        model.InventoryCount += item.ProductCount;
        await _prodcutRepository.UpdateAsync(model);
    }

    private async Task<string> CheckSupilerChange(string id,UpdatePuraseCommand request)
    {
    
        var supier = await _supilerInfoRepository.GetByKey(request.SupplierId);
        if (supier == null)
        {
            var sucode = await _ruleManager.getNextRuleNumber(RuleType.supplier);
            var supileinfo = new SupplierInfo
            {
                SupName =request.SupileName,
                SupNumber = sucode,
                SupTel = request.InPhone,
                SupTelT = request.InPhone,
                SupPhone = request.InPhone,
                SupPhoneT = request.InPhone,
                ProviderUser =request.InUser,
                Enable = true,
                ProviderUserT = request.InUser,
                CreateUser = _userHelper.LoginName
            };
            await _supilerInfoRepository.AddAsync(supileinfo);
            return supileinfo.Id;
        }
       
        return supier.Id;
    }
}