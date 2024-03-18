using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LCPC.Infrastructure.Repositories
{
    public class SqlDapper : ISqlDapper
    {
        private readonly string _connectionString;
        public SqlDapper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                 var result =await con.QueryAsync<T>(sql, ConvertParameters(param));
                 return result.ToList();
            }
        }

        public async Task<List<T>> QueryAsync<T>(string sql, object param, CommandType commandType = CommandType.Text)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var result =await con.QueryAsync<T>(sql, ConvertParameters(param), null, null, commandType);
                return result.ToList();
            }
        }
        
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param, CommandType commandType = CommandType.Text)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var  result =await con.QueryMultipleAsync(sql, ConvertParameters(param), null, null, commandType);
                return result;
            }
        }


        public async Task<int> QueryCountAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var result = await con.QueryFirstAsync<int>(sql, ConvertParameters(param));
                return result;
            }
        }
        public async Task<long> QueryLongCountAsync(string sql, object param = null)
        {
  
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var result = await con.QueryFirstAsync<long>(sql, ConvertParameters(param));
                return result;
            }
        }
        public async Task<T> QueryFirstAsync<T>(string sql, object param = null)
        {
             using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                var result = await con.QueryFirstOrDefaultAsync<T>(sql, ConvertParameters(param));
                return result;
            }
        }
        
        public async Task UpdateAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                
                await con.OpenAsync();
                await con.ExecuteAsync(sql, ConvertParameters(param));
                await Task.CompletedTask;
            }
        }
        public async Task<T> OpenConnectionAsync<T>(Func<IDbConnection,T > func)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                return func(con);
            }
        }

        public async Task<object> ExecuteScalerAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                
                await con.OpenAsync();
               return   await con.ExecuteScalarAsync(sql, ConvertParameters(param));
               
            }
        }

        private object ConvertParameters(object param)
        {
            var dymic =new DynamicParameters();
            if (param != null)
            {
                var t = param.GetType();
                if (t.Name.Contains("Anonymous"))
                    return param;
                if (typeof(Hashtable) == t)
                {
                    Hashtable hs = (Hashtable)param;
                    foreach (var key in hs.Keys)
                    {
                        object value = hs[key];
                        dymic.Add(key.ToString(),value);
                    }

                    return dymic;
                } 
                if (typeof(Dictionary<string, object>) == t)
                {
                    Dictionary<string, object> hs = (Dictionary<string, object>)param;
                    foreach (var key in hs.Keys)
                    {
                        object value = hs[key];
                        dymic.Add(key.ToString(),value);
                    }

                    return dymic;
                }
                return param;
            }
            return dymic;
        }

        public IDbConnection changeDbConnection(string ConString)
        {
            SqlConnection _sqlConnection = null;
            try
            {
                _sqlConnection = new   SqlConnection(ConString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (_sqlConnection != null)
                {
                    _sqlConnection.Close();
                    _sqlConnection.Dispose();
                }
                  
                
            }
            return _sqlConnection;
        }
    }
}