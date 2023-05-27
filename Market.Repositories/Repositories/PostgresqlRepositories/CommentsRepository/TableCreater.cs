using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository
{
    public class TableCreater : ITableCreater
    {
        public static string TableName { get { return "Comments"; } }

        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create()
        {
            string sql = $"CREATE  TABLE IF NOT EXISTS  {TableName}" +
    "(" +
    $" {Id} SERIAL PRIMARY KEY, " +
    $" {DignityColumnName} text, " +
    $" {Flaws} text, " +
    $" {Comment} text, " +
    $" {ProductId} bigint REFERENCES {ProductsRepository.TableCreater.TableName} ({ProductsRepository.Columns.Id}) NOT NULL,  " +
    $" {UserIdCol} text REFERENCES {UsersRepository.TableCreater.Table} ({UsersRepository.Columns.UserId}),  " +
    $" {Likes} integer,  " +
    $" {Stars} integer" +
    ");";
            _connection.Query(sql);

        }
    }
}
