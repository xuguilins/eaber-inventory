namespace LCPC.Infrastructure.Repositories;

public class PurchaseOutOrderDetailRepository:Repository<PurchaseOutOrderDetail>,IPurchaseOutOrderDetailRepository
{
    public PurchaseOutOrderDetailRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}