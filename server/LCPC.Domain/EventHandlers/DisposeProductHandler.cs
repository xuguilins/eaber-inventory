using LCPC.Domain.EventHandlers.EventDatas;
using Microsoft.AspNetCore.DataProtection;

namespace LCPC.Domain.EventHandlers;

public class DisposeProductHandler:INotificationHandler<DisposeProduct>
{
    private readonly IProdcutRepository _prodcutRepository;
    public DisposeProductHandler(IProdcutRepository prodcutRepository)
    {
        _prodcutRepository = prodcutRepository;
    }
    public async Task Handle(DisposeProduct notification, CancellationToken cancellationToken)
    {
        var product = await _prodcutRepository.GetByKey(notification.ProductId);
        if (product != null)
        {
            if (notification.Option == Option.Cancale)
            {
                product.InventoryCount -= notification.Count;
                await _prodcutRepository.UpdateProductCount(product.Id,product.InventoryCount);
            }
            else
            {
                product.InventoryCount += notification.Count;
                await _prodcutRepository.UpdateProductCount(product.Id,product.InventoryCount);
            }
        }
        

        await Task.CompletedTask;
    }
}