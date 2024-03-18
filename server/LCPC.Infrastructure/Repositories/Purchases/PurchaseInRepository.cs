using LCPC.Domain;
using LCPC.Domain.QueriesDtos;
using LCPC.Share.Response;

namespace LCPC.Infrastructure.Repositories;

public class PurchaseInRepository:Repository<PurchaseInOrder>,IPurchaseInRepository
{
    private readonly AdminDbContext _context;
    public PurchaseInRepository(AdminDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
        _context = context;
    }

    public async Task<List<PurchaseInOrder>> GetPuraseInOrders(string[] ids)
    {
        var model = await _context.Set<PurchaseInOrder>()
            .Include(d => d.SupplierInfo)
            .Include(d => d.PurchaseInDetails)
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();
        return model;
    }

    public async Task<PurchaseInOrder> GetPuraseInOrder(string id)
    {
        var model = await _context.Set<PurchaseInOrder>()
            .Include(d => d.SupplierInfo)
            .Include(d => d.PurchaseInDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
        return model;

    }


}