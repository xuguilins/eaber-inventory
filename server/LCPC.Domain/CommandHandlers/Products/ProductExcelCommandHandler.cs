using System.Data;

namespace LCPC.Domain.CommandHandlers;

public class ProductExcelCommandHandler : IRequestHandler<ProductExcelCommand<ProductExcelDto>, ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    private readonly ICatetoryRepository _catetoryRepository;
   
    private readonly IRuleManager _ruleManager;
    private readonly ISupilerInfoRepository _supilerInfoRepository;
    private readonly ISystemDicInfoRepository _systemDicInfoRepository;
    public ProductExcelCommandHandler(IProdcutRepository prodcutRepository,
        ICatetoryRepository catetoryRepository,
       
        IRuleManager ruleManager,
        ISupilerInfoRepository supilerInfoRepository,
        ISystemDicInfoRepository systemDicInfoRepository
    )
    {
        _prodcutRepository = prodcutRepository;
        _catetoryRepository = catetoryRepository;
        _systemDicInfoRepository = systemDicInfoRepository;
        _ruleManager = ruleManager;
        _supilerInfoRepository = supilerInfoRepository;
    }

    public async Task<ReturnResult> Handle(ProductExcelCommand<ProductExcelDto> request,
        CancellationToken cancellationToken)
    {
        if (!request.Products.Any())
            throw new Exception("导入的数据无效，请检查导入文件");
        // 加载所有分类
        var allCates = _catetoryRepository.GetEntities.Select(x =>
            new
            {
                CateName = x.CateName,
                Id = x.Id
            }).ToList();
        var allUnits = _systemDicInfoRepository.GetEntities.Select(x =>
            new
            {
                UnitName = x.DicCode,
                Id = x.Id
            }
        ).ToList();
        var allSupilers = _supilerInfoRepository.GetEntities.Select(x => new
        {
            Id = x.Id,
            SupileName = x.SupName
        }).ToList();
        var allProducts = _prodcutRepository.GetEntities.Select(x => x.ProductName).ToList();
        var defaultCate = allCates.FirstOrDefault(x => x.CateName == "其他类别");
        var defaultUnit = allUnits.FirstOrDefault(x => x.UnitName == "个");
        var defaultSup = allSupilers.FirstOrDefault();
        bool haveProduct = allProducts.Any();
        List<ProductInfo> infos = new List<ProductInfo>();
        int count = 0;
      //  int i = 0;
        int valueCount = request.Products.Count;
        var listNumbers = await _ruleManager.createMuchNumber(RuleType.Product, valueCount);
        foreach (var item in request.Products)
        {
            if (!string.IsNullOrWhiteSpace(item.ProductName))
            {
                bool model = false;
                if (haveProduct)
                    model = allProducts.Any(v => v == item.ProductName);
                if (!model)
                {
                    
                   // var numbers = await _ruleManager.getNextRuleNumber(RuleType.Product);
                    ProductInfo product = new ProductInfo();
                    product.ProductName = item.ProductName;
                    product.ProductModel = item.ProductModel;
                    product.ProductCode = listNumbers[count];
                    product.InitialCost = item.InitialCost;
                    product.ConversionRate = item.ConversionRate;
                    product.CateId = defaultCate.Id;
                    product.UnitId = defaultUnit.Id;
                    product.SupilerId = defaultSup.Id;
                    var cate = allCates.FirstOrDefault(x => x.CateName == item.Cate);
                    if (cate != null)
                        product.CateId = cate.Id;
                    var unit = allUnits.FirstOrDefault(x => x.UnitName == item.Unit);
                    if (unit != null)
                        product.UnitId = unit.Id;
                    var sup = allSupilers.FirstOrDefault(x => x.SupileName.Equals(item.SupileName));
                    if (sup != null)
                        product.SupilerId = sup.Id;
                    product.Purchase = item.Purchase;
                    product.Wholesale = item.Wholesale;
                    int InventoryCount = 0;
                    if (int.TryParse(item.InventoryCount, out InventoryCount))
                        product.InventoryCount = InventoryCount;
                    int MaxStock = 0;
                    if (int.TryParse(item.MaxStock, out MaxStock))
                        product.MaxStock = MaxStock;
                    int MinStock = 0;
                    if (int.TryParse(item.MinStock, out MinStock))
                        product.MinStock = MinStock;
                    product.InventoryCount = InventoryCount;
                    product.SellPrice = item.SellPrice;
                    // product.Remark = item.Remark;
                     infos.Add(product);
                    count++;
                   // i++;
                }
            }
        }
        await _prodcutRepository.AddRangeAsync(infos);
        int result = await _prodcutRepository.UnitOfWork.SaveChangesAsync();
        return result >= 0
            ? new ReturnResult(true, null, "成功导入【" + count + "】条数据")
            : new ReturnResult(false, null, "导入失败");
    }
}