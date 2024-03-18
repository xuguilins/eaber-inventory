using System;
using System.Data;
using LCPC.Share;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace LCPC.Infrastructure.Repositories
{
    
    public class Repository<T> : IRepository<T>
        where T : EntityBase
    {
        public event EventHandler<T> Handler;
        private readonly AdminDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IServiceProvider _serviceProvider;
        private readonly CacherHelper _cacherHelper;
        public Repository(AdminDbContext context, IServiceProvider serviceProvider)
        {
            _context = context; _dbSet = _context.Set<T>();
            _serviceProvider = serviceProvider;
            _cacherHelper = _serviceProvider.GetRequiredService<CacherHelper>();
        }
        public IUnitOfWork UnitOfWork => _context;

        public IQueryable<T> GetEntitiesTracking => _dbSet.AsTracking<T>();

        public IQueryable<T> GetEntities
        {
            get
            {
                // var obj = _cacherHelper.GetQuery<T>();
                // if (obj != null)
                //     return obj;
                return  _dbSet.AsNoTracking<T>();
            }
           
            
        }

        public IQueryable<T> GetQueryable
        {
            get
            {
              return    _dbSet.AsNoTracking();
            }
        }


        public IQueryable<T> GetQueryTrackAble => _dbSet.AsTracking();

        public async Task<T> AddAsync(T data)
        {
          //  _cacherHelper.Set<T>(data);
            var entity = await _dbSet.AddAsync(data);
            return entity.Entity;
        }
        public async Task AddRangeAsync(List<T> data)
        {

            await _dbSet.AddRangeAsync(data);
            await Task.CompletedTask;
        }
        public async Task<T> GetByKey(string id)
        {

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            Handler?.Invoke(this,entity);
            return entity;
        }

        public async Task<T> FindEntity(Expression<Func<T, bool>> expression)
        {
            var model = await _dbSet.FirstOrDefaultAsync(expression);
            return model;
        }

        public async Task RemoveAsync(T data)
        {
            _dbSet.Remove(data);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(IEnumerable<T> data)
        {
            _dbSet.RemoveRange(data);
            await Task.CompletedTask;
        }

        public async Task<T> UpdateAsync(T data)
        {
            var entity = _dbSet.Update(data);
            return await Task.FromResult(entity.Entity);
        }
        public async Task<List<T>> GetEntityPageAsync(int page, int pageSize, Expression<Func<T, bool>> expression)
        {
            _ = new List<T>();
            List<T> list;
            if (expression == null)
                list = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            else
                list = await _dbSet.Where(expression).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return list;
        }

        public async Task<long> GetEntityCountAsync(Expression<Func<T, bool>> expression)
        {
            long totalCount;
            if (expression == null)
                totalCount = await _dbSet.LongCountAsync();
            else
                totalCount = await _dbSet.LongCountAsync(expression);
            return totalCount;
        }

        public async Task<List<T>> GetEntitiesAsync(Expression<Func<T, bool>> expression)
        {
            var list = await _dbSet.Where(expression).ToListAsync();
            return list;
        }
        public async  Task<List<T>> GetTrackEntitiesAsync(Expression<Func<T, bool>> expression){
            var list = await _dbSet.AsTracking<T>().Where(expression).ToListAsync();
            return list;
        }
        public async Task<int> AddBlockAsync(IEnumerable<T> data, string[] ignoreColumns=null, Action<SqlBulkCopy> action = null)
        {
            int result = 0;
            try
            {
                SqlConnection connection = _context.Database.GetDbConnection() as SqlConnection;
                await connection.OpenAsync();
                SqlTransaction _sqlTransaction = null;
                var _efTransaction = _context.GetCurrentTransaction();
                if (_efTransaction != null)
                    _sqlTransaction = _efTransaction.GetDbTransaction() as SqlTransaction;
                using SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, _sqlTransaction);
                var table = createTable(data,ignoreColumns);
                var dt = table.Item1;
                bulkCopy.DestinationTableName = dt.TableName;
                bulkCopy.BatchSize = 10000;
                if (action != null)
                {
                    action(bulkCopy);
                }
                else
                {
                    var list = table.Item2;
                    if (list.Any())
                    {
                        list.ForEach(map =>
                        {
                            bulkCopy.ColumnMappings.Add(map, map);
                        });
                    }
                }

                await bulkCopy.WriteToServerAsync(dt);
                result = 1;
            }
            catch (Exception ex)
            {
                await _context.GetCurrentTransaction().RollbackAsync();
                await _context.GetCurrentTransaction().DisposeAsync();
            }
            return result;

        }

        public async Task<int> AddBlockAsync(DataTable data,  string tableName,  string[] ignoreColumns = null, Action<SqlBulkCopy> action = null)
        {
            int result = 0;
            try
            {
                SqlConnection connection = _context.Database.GetDbConnection() as SqlConnection;
                await connection.OpenAsync();
                SqlTransaction _sqlTransaction = null;
                var _efTransaction = _context.GetCurrentTransaction();
                if (_efTransaction != null)
                    _sqlTransaction = _efTransaction.GetDbTransaction() as SqlTransaction;
                using SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, _sqlTransaction);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = 10000;
                if (action != null)
                {
                    action(bulkCopy);
                }
                else
                {
                    foreach (DataColumn map in data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(map.ColumnName, map.ColumnName);
                    }
                }
                await bulkCopy.WriteToServerAsync(data);
                result = 1;
            }
            catch (Exception ex)
            {
                await _context.GetCurrentTransaction().RollbackAsync();
                await _context.GetCurrentTransaction().DisposeAsync();
            }
            return result;
        }
        private (DataTable, List<string>) createTable(IEnumerable<T> data,string[] ignoreColumns)
        {

            List<string> list = new List<string>();
            var typeInfo = typeof(T);
            var props = typeInfo.GetProperties()
            .ToList();
            var table = new DataTable
            {
                TableName = typeInfo.Name
            };
            props.ForEach(col =>
            {
                if (ignoreColumns != null && ignoreColumns.Length > 0)
                {
                    if (!ignoreColumns.Contains(col.Name))
                    {
                        table.Columns.Add(col.Name, col.PropertyType);
                        list.Add(col.Name);
                    }
                         
                }
                else
                {
                    table.Columns.Add(col.Name, col.PropertyType);
                    list.Add(col.Name);
                }
             
               
            });
            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                props.ForEach(col =>
                {
                    if (ignoreColumns != null && ignoreColumns.Length > 0)
                    {
                        if (!ignoreColumns.Contains(col.Name))
                            row[col.Name] = col.GetValue(item);
                    }
                    else
                    {
                        row[col.Name] = col.GetValue(item);
                    }
                });
                table.Rows.Add(row);
            }
            return (
               table, list
            );
        }
    }
}

