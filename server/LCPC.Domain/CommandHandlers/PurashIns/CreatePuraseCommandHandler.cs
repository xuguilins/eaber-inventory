using System.Data;
using Dapper;
using LCPC.Domain.Commands;
using LCPC.Domain.EventHandlers.EventDatas;
using LCPC.Domain.Queries;
using TinyPinyin;

namespace LCPC.Domain.CommandHandlers;

public class CreatePuraseCommandHandler:IRequestHandler<CreatePuraseCommand,ReturnResult>
{
    private readonly ISqlDapper _sqlDapper;
    private readonly IProductQueries _productQueries;
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IPurchaseInRepository _purchaseInRepository;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
 
    
    private readonly IRuleManager _ruleManager;
    private readonly UserHelper _userHelper;
    private readonly IMediator _mediator;
    public CreatePuraseCommandHandler(ISqlDapper sqlDapper,
        IProductQueries productQueries,
        IProdcutRepository prodcutRepository,
        IPurchaseInRepository purchaseInRepository,
        ISupilerInfoRepository supilerInfoRepository,
        IRuleManager ruleManager,
        UserHelper userHelper,
        IMediator mediator
        )
    {
        _sqlDapper = sqlDapper;
        _productQueries = productQueries;
        _prodcutRepository = prodcutRepository;
        _purchaseInRepository = purchaseInRepository;
        _ruleManager = ruleManager;
        _supilerInfoRepository = supilerInfoRepository;
        _userHelper = userHelper;
        _mediator = mediator;
    }
    public async Task<ReturnResult> Handle(CreatePuraseCommand request, CancellationToken cancellationToken)
    {

        var products = await _prodcutRepository.GetEntitiesAsync(d => d.Enable);
        var pushCode = await _ruleManager.getNextRuleNumber(RuleType.PurchaseIn);
        // 创建进货单据
        PurchaseInOrder order = new PurchaseInOrder
        {
            InOrderTime = request.InOrderTime,
            InCount = request.InCount,
            ChannelType = request.ChannelType,
            Logistics = request.Logistics,
            InUser = request.InUser,
            InPhone = request.InPhone,
            SupplierId = request.SupplierId,
            InPrice = request.InPrice,
            InOStatus = InOStatus.APAY,
            Remark = request.Remark,
            PurchaseCode = pushCode,
            CreateUser =  _userHelper.LoginName
        };
         
        order.SupplierId = await CreateSupiler(request);
        var detials = request.PrdocutDetail;
        foreach (var item in detials)
        {
            PurchaseInDetail detail = new PurchaseInDetail();
            detail.ProductCode = item.ProductCode;
            detail.ProductAll = (item.ProductCount * item.ProductPrice);
            detail.ProductModel = item.ProductModel;
            detail.ProductName = item.ProductName;
            detail.ProductCount = item.ProductCount;
            detail.ProductPrice = item.ProductPrice;
            detail.PurchaseInId = order.Id;
            detail.Remark = item.Remark;
            detail.CreateUser = order.CreateUser;
            var model = products.FirstOrDefault(d => d.ProductCode == item.ProductCode);
            if (model != null)
            {
                // 商品存在
                detail.ProductId = model.Id;
                await UpdateProduct(model, item);
                // 增加库存 
                await _mediator.Publish(new DisposeProduct
                {
                    Option = Option.Complete,
                    Count = item.ProductCount,
                    ProductId = model.Id
                });
            }
            else
            {
                //插入商品
                var code = await _ruleManager.getNextRuleNumber(RuleType.Product);
                detail.ProductId = await CreateProduct(item, order.SupplierId, code);
                detail.ProductCode = code;
            }
            order.PurchaseInDetails.Add(detail);
        }
        order.InPrice = order.PurchaseInDetails.Sum(d => d.ProductAll);
        order.InCount = order.PurchaseInDetails.Sum(d => d.ProductCount);
        await _purchaseInRepository.AddAsync(order);
        int result = await _purchaseInRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, "采购单生成成功")
            : new ReturnResult(false, null, "采购单生成失败");
    }

    private async Task<string>  CreateProduct(PurashDetailDto item,string  SupplierId,string code)
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
            CreateUser = _userHelper.LoginName,
            NameSpell = PinyinHelper.GetPinyinInitials(item.ProductName)
        };
        await _prodcutRepository.AddAsync(product);
        return product.Id;
    }

    private async Task UpdateProduct(ProductInfo model,PurashDetailDto item)
    {
        model.Wholesale = item.ProductWocost;
        model.SellPrice = item.SellPrice;
        model.InitialCost = item.ProductIncost;
        model.Purchase = item.ProductPrice;
        await _prodcutRepository.UpdateAsync(model);
    }

    private async Task<string> CreateSupiler(CreatePuraseCommand request)
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

        return request.SupplierId;
    }
     
}