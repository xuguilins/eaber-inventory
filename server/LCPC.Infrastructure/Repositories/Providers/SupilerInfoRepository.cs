using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Infrastructure.Repositories
{
    public class SupilerInfoRepository : Repository<SupplierInfo>, ISupilerInfoRepository
    {
        public SupilerInfoRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}