using System.Data;
using LCPC.Domain.Entities;
using LCPC.Domain.IRepositories;
using LCPC.Infrastructure.EntityConfigurations;
using LCPC.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace LCPC.Infrastructure.Cores;

public class AdminDbContext : DbContext, IUnitOfWork
{
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    private IDbContextTransaction _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;
    public AdminDbContext(DbContextOptions<AdminDbContext> options)
    :base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.AddInterceptors(new IntertorCommand())
            .EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfiguration(new CateInfoConfiguration());
        modelBuilder.ApplyConfiguration(new RuleInfoConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierInfoConfigutaion());
        modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
        modelBuilder.ApplyConfiguration(new ProductInfoConfiguration());
        modelBuilder.ApplyConfiguration(new OrderInfoConfiguration());
        modelBuilder.ApplyConfiguration(new OrderInfoDetailConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseInOrderConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseInDetailConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOutOrderConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOutOrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerInfoConfiguration());
        modelBuilder.ApplyConfiguration(new SystemDicInfoConfiguration());
        modelBuilder.ApplyConfiguration(new ExtraOrderConfiguration());
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;
        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction)
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}