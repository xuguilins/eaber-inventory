using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCPC.Domain.IRepositories
{
    public interface IOrderRepository:IRepository<OrderInfo>
    {
        Task<List<OrderInfo>> GetOrderInfo(string[] ids);

        Task<OrderInfo> GetSignleOrder(string id);
    }
}