namespace LCPC.Infrastructure.Repositories;

public class PurchaseOutOrderRepository:Repository<PurchaseOutOrder>,IPurchaseOutOrderRepository
{
    public PurchaseOutOrderRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}