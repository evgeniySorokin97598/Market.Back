using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.UsersRepository.Columns;
namespace Market.Repositories.Repositories.PostgresqlRepositories.UsersRepository
{
    public class TableCreater : ITableCreater
    {
        public static string Table = "Users";

        private IDbConnection _connection;

        public TableCreater(IDbConnection connection)
        {
            _connection = connection;

        }

        public void Create()
        {
            string sql = $"CREATE  TABLE IF NOT EXISTS {Table} (" +
                 $" {UserId} text PRIMARY KEY," +
                 $" {Nickname} text" +
                 //$" {UserId} text UNIQUE" +
                 $")";
            _connection.Query(sql);
        }
    }
}
