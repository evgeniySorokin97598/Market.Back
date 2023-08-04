using Dapper;
using DataBaseLib.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLib.Providers
{
    public class PostgresqlProvider : IDbProvider
    {
        private DataBaseConfig _config;
        public PostgresqlProvider(DataBaseConfig DataBaseConfig) {
            _config = DataBaseConfig;
        }
        public Task<IEnumerable<T>> QueryAsync<T>(string query, object obj = null)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString())) {
               return  connection.QueryAsync<T>(query, obj);
            }
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string query, object obj = null)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                return connection.QueryAsync(query, obj);
            }
        }

        private string GetConnectionString() {
           
            return $"Host={_config.Host}:{_config.Port};Database = {_config.DataBase}; Username={_config.Username};Password={_config.Password}"; 
        }
    }
}
