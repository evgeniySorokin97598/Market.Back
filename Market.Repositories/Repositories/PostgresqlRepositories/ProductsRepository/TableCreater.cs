using Dapper;
using Market.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.ProductsRepository.Columns;
namespace Market.Repositories.Repositories.PostgresqlRepositories.ProductsRepository
{
    internal class TableCreater : ITableCreater
    {
        public static string TableName { get { return "products"; } }
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
    $" {Name} text," +
    $" {Description} Text default 'Нет описания'," +
    $" {Price} integer NOT NULL," +
    $" {Image} Text ," +
    $" {Quantity} integer, " +
    $" {Brend} text NOT NULL," +
    $" {ImageUrl} text," +
    $" {SubcategoryId} INTEGER REFERENCES {SubcategoryRepository.TableCreater.TableName} ({SubcategoryRepository.Columns.IdColumnName}) NOT NULL, " +
    $" {СharacteristicId} SERIAL UNIQUE " +
");";

            _connection.Query(sql);
        }
    }
}
