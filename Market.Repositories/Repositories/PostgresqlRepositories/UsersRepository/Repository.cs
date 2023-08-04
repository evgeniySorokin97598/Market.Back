using Dapper;
using DataBaseLib.Interfaces;
using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Market.Repositories.Repositories.PostgresqlRepositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.UsersRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.UsersRepository
{
    public class Repository : BaseRepository, IUsersRepository
    {
         

        private string Table { get { return TableCreater.Table; } }

        public Repository(IDbProvider connection) : base(connection)
        {
             
        }

        public async Task<long> GetUser(UserInfo info)
        {
            string sql = $"SELECT count(*) FROM {Table} where {UserId} = @user";
            var result = (await _connection.QueryAsync<long>(sql, new
            {
                user = info.Id
            })).FirstOrDefault();
            if (result == 0) // если пользователь раньше не был создан
            {
                return await AddAsync(info);
            }
            return result;
        }

        public async Task<long> AddAsync(UserInfo info)
        {
            string sql = $"INSERT INTO {Table} ({UserId},{Nickname}) VALUES (@UserId,@nick) ";
            var result = (await _connection.QueryAsync<long>(sql, new
            {
                UserId = info.Id,
                nick = info.Username
            })).FirstOrDefault();
            return result;
        }
    }
}
