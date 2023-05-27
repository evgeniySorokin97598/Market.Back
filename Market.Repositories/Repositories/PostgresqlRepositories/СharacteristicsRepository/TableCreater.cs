using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.СharacteristicsRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.СharacteristicsRepository
{
    internal class TableCreater : ITableCreater
    {
        public static string TableName = "Сharacteristics";
        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }
        public void Create()
        {
            string sql = $"CREATE  TABLE IF NOT EXISTS  {TableName}" +
"(" +
    $" {Id} SERIAL PRIMARY KEY," +
    $" {СharacteristicName} text ," +
    $" {Сharacteristic} text," +
    $" {TypeСharacteristicsId} bigint REFERENCES {TypeСharacteristicsRepository.TableCreater.TableName} ({TypeСharacteristicsRepository.Columns.Id}) NOT NULL" +
" ); ";
            _connection.Query(sql);
        }
    }
}
