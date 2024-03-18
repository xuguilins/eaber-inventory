using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace LCPC.Domain.IRepositories
{
    public interface ISqlDapper
    {
        Task<List<T>> QueryAsync<T>(string sql, object param = null);
        Task<List<T>> QueryAsync<T>(string sql, object param, CommandType commandType = CommandType.Text);
        Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param, CommandType commandType = CommandType.Text);
           

        Task<T> QueryFirstAsync<T>(string sql, object param = null);

        Task<int> QueryCountAsync(string sql, object param = null);
        Task<long> QueryLongCountAsync(string sql, object param = null);

        Task UpdateAsync(string sql, object param = null);

        Task<T> OpenConnectionAsync<T>(Func<IDbConnection, T> func);

        Task<object> ExecuteScalerAsync(string sql, object param = null);

        IDbConnection changeDbConnection(string ConString);
    }
}