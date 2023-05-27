using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.CategoriesRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CategoriesRepository
{
    public class TableCreater : ITableCreater
    {
        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }
        public static string TableName = "Categories";
        public void Create()
        {
            string sql = $"CREATE TABLE IF NOT EXISTS {TableName} " +
"(" +
   $" {IdColumn}    BIGSERIAL PRIMARY KEY," +
  $"  {UrlIconColumnName} Text," +
   $" {ColumnName}  Text NOT NULL " +
"); ";
            _connection.Query(sql);
        }
    }
}
