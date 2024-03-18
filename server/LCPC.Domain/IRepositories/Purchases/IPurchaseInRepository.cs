namespace LCPC.Domain.IRepositories;

public interface IPurchaseInRepository:IRepository<PurchaseInOrder>
{
    Task<PurchaseInOrder> GetPuraseInOrder(string id);

    Task<List<PurchaseInOrder>> GetPuraseInOrders( string[] ids);
}