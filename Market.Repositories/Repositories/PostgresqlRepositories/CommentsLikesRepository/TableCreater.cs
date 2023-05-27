using Dapper;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository
{
    public class TableCreater : ITableCreater
    {
        public static string TableName = "CommentsLikes";
        private IDbConnection _connection;
        public TableCreater(IDbConnection connection)
        {
            _connection = connection;
        }
        public void Create()
        {
            string sql = $"CREATE TABLE IF NOT EXISTS {TableName} (" +
                $"{Id} SERIAL PRIMARY KEY,"+
                $" {CommentId} bigint REFERENCES {CommentsRepository.TableCreater.TableName} ({CommentsRepository.Columns.Id}) NOT NULL," +
                $" {UserId} text REFERENCES {UsersRepository.TableCreater.Table} ({UsersRepository.Columns.UserId})  NOT NULL" +
                $") ";
            _connection.Query(sql);
        }
    }
}
