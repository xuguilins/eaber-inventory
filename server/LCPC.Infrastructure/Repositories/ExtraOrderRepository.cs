namespace LCPC.Infrastructure.Repositories;

public class ExtraOrderRepository:Repository<ExtraOrder>,IExtraOrderRepository
{
    public ExtraOrderRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}