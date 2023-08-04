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
        public PostgresqlProvider(DataBaseConfig DataBaseConfig)
        {
            _config = DataBaseConfig;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object obj = null)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                return await connection.QueryAsync<T>(query, obj);
            }
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string query, object obj = null)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                return await connection.QueryAsync(query, obj);
            }
        }

        private string GetConnectionString()
        {

            return $"Host={_config.Host};Port ={_config.Port};Database = {_config.DataBase}; Username={_config.Username};Password={_config.Password}";
        }
    }
}
