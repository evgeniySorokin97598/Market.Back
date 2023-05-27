using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.TypeСharacteristicsRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.TypeСharacteristicsRepository
{
    public class TableCreater : ITableCreater
    {
        public static string TableName = "TypeСharacteristics";
        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }
        public void Create()
        {
            string sql = $"CREATE  TABLE IF NOT EXISTS  {TableName}"+
"("+
    $" {Id} SERIAL PRIMARY KEY,"+
    $" {TypeСharacteristicsName} text,"+
    $" {ProductId} bigint REFERENCES {ProductsRepository.TableCreater.TableName} ({ProductsRepository.Columns.СharacteristicId}) NOT NULL  "+
");";
            _connection.Query(sql);
        }
    }
}
