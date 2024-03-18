using System.Data;
using LCPC.Domain.Entities;
using Microsoft.Data.SqlClient;
namespace LCPC.Domain.IRepositories;

public interface IRepository<T>
where T:EntityBase
{

    public event EventHandler<T> Handler;
    IQueryable<T> GetEntities { get; }
    IQueryable<T> GetEntitiesTracking { get;}
    
    IQueryable<T> GetQueryable { get; }

    IQueryable<T> GetQueryTrackAble { get;  }
    
    IUnitOfWork UnitOfWork { get; }

    Task<T> AddAsync(T data);

    Task AddRangeAsync(List<T> data);

    Task<T> UpdateAsync(T data);

    Task RemoveAsync(T data);

    Task RemoveAsync(IEnumerable<T> data);

    Task<T> GetByKey(string id);

    Task<T> FindEntity(Expression<Func<T, bool>> expression);

    Task<List<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression);
    Task<List<T>> GetTrackEntitiesAsync(Expression<Func<T, bool>> expression);

    Task<List<T>> GetEntityPageAsync(int page, int pageSize, Expression<Func<T, bool>> expression=null);

    Task<long> GetEntityCountAsync(Expression<Func<T, bool>> expression=null);

    Task<int> AddBlockAsync(IEnumerable<T> data, string[] ignoreColumns=null,Action<SqlBulkCopy> action = null );
    
    Task<int> AddBlockAsync(DataTable data, string tableName, string[] ignoreColumns=null,Action<SqlBulkCopy> action = null );
 
}