using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Infrastructure.Repositories
{
    public class OrderRepository : Repository<OrderInfo>, IOrderRepository
    {
        private readonly AdminDbContext _adminDbContext;
        public OrderRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
            _adminDbContext = context;
        }

        public async Task<List<OrderInfo>> GetOrderInfo(string[] ids)
        {
            var orders = await _adminDbContext.Set<OrderInfo>()
                .Include(x => x.OrderInfoDetails)
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();
            return orders;
        }

        public async Task<OrderInfo> GetSignleOrder(string id)
        {
            var order = await _adminDbContext.Set<OrderInfo>()
                .Include(x => x.OrderInfoDetails)
                .FirstOrDefaultAsync(d => d.Id.Equals(id));
            return order;
        }
    }
}