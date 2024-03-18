namespace LCPC.Infrastructure.Repositories;

public class CustomerInfoRepository:Repository<CustomerInfo>,ICustomerInfoRepository
{
    public CustomerInfoRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}