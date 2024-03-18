namespace LCPC.Domain.CommandHandlers;

public class DeletePuraseCommandHandler:IRequestHandler<DeletePuraseCommand,ReturnResult>
{
    private readonly IProdcutRepository _prodcutRepository;
    private readonly IPurchaseInRepository _purchaseInRepository;
    public DeletePuraseCommandHandler(IProdcutRepository prodcutRepository,IPurchaseInRepository purchaseInRepository)
    {
        _prodcutRepository = prodcutRepository;
        _purchaseInRepository = purchaseInRepository;
    }

    public async Task<ReturnResult> Handle(DeletePuraseCommand request, CancellationToken cancellationToken)
    {
        var list = await _purchaseInRepository.GetPuraseInOrders(request.Ids);
        await _purchaseInRepository.RemoveAsync(list);
        Dictionary<string, int> dics = new Dictionary<string, int>();
        //释放库存
        foreach (var item in list)
        {
            foreach (var product in item.PurchaseInDetails)
            {
                var id = product.ProductId;
                if (dics.ContainsKey(id))
                {
                    int count = dics[id];
                    dics[id] = product.ProductCount + count;
                }
                else
                {
                    dics.Add(id, product.ProductCount);
                }
               
            }
        }

        await DisposeProduct(dics);
        int result = await _purchaseInRepository.UnitOfWork.SaveChangesAsync();
        return result > 0
            ? new ReturnResult(true, null, MessageHelper.DeleteMessage(list.Count))
            : new ReturnResult(false, null, MessageHelper.DeleteMessage(list.Count, false));
    }

    private async Task DisposeProduct(Dictionary<string, int> dics)
    {
        var ids = dics.Select(d => d.Key).ToList();
        var products = await _prodcutRepository.GetEntitiesAsync(d => ids.Contains(d.Id));
        foreach (var product in products)
        {
            product.InventoryCount -= dics[product.Id];
            await _prodcutRepository.UpdateAsync(product);
        }
    }
}