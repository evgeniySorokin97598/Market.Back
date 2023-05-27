using Dapper;
using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Market.Repositories.Repositories.PostgresqlRepositories.SubcategoryRepository.Columns;
namespace Market.Repositories.Repositories.PostgresqlRepositories.SubcategoryRepository
{
    internal class TableCreater : ITableCreater
    {
        public static string TableName = "subcategory";
        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create()
        {
            string sql = $"CREATE TABLE IF NOT EXISTS {TableName}" +
$"(" +
   $" {IdColumnName}    BIGSERIAL PRIMARY KEY," +
   $" {UrlIconcColumnName} Text," +
   $" {NameColumnName}  Text NOT NULL," +
   $" {CategoryIdColumnName} INTEGER REFERENCES {CategoriesRepository.TableCreater.TableName} ({CategoriesRepository.Columns.IdColumn}) NOT NULL" +
");";
            _connection.Query(sql);
        }
    }
}
