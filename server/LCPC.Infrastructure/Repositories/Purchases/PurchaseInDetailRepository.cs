namespace LCPC.Infrastructure.Repositories;

public class PurchaseInDetailRepository:Repository<PurchaseInDetail>,IPurchaseInDetailRepository
{
    public PurchaseInDetailRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}