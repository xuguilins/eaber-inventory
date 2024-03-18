namespace LCPC.Infrastructure.Repositories;

public class SystemDicInfoRepository:Repository<SystemDicInfo>,ISystemDicInfoRepository
{
    public SystemDicInfoRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}