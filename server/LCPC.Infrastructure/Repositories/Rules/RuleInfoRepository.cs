using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Infrastructure.Repositories
{
    public class RuleInfoRepository : Repository<RuleInfo>, IRuleInfoRepository
    {
        public RuleInfoRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }
    }
}