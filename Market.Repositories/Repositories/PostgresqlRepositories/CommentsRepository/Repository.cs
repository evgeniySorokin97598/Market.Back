using Dapper;
using DataBaseLib.Interfaces;
using Market.Entities.Dto;
using Market.Entities.Requests;
using Market.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository
{
    public class Repository : BaseRepository, ICommentsRepository
    {


        private IUsersRepository _usersRepository;
        private string TableName = TableCreater.TableName;
        public Repository(IDbProvider connection, IUsersRepository usersRepository) : base(connection)
        {

            _usersRepository = usersRepository;
        }

        public async Task AddAsync(AddCommentRequest request, UserInfo info)
        {
            try
            {
                await _usersRepository.AddAsync(info);
            }
            catch (Exception ex)
            {

            }

            if (!string.IsNullOrEmpty(request?.Comment) || !string.IsNullOrEmpty(request?.Dignity) || !string.IsNullOrEmpty(request?.Flaws))
            {
                //long id = await _usersRepository.GetUser(info);
                string sql = $"INSERT INTO {TableName} ({DignityColumnName},{Flaws},{Comment},{ProductId},{UsersRepository.Columns.UserId},{Stars}) VALUES (@Dignity,@Flaws,@Comment,@ProductId,@userId,@stars)";
                await _connection.QueryAsync(sql, new
                {
                    request.Dignity,
                    request.Flaws,
                    request.Comment,
                    request.ProductId,
                    userId = info.Id,
                    stars = request.Stars,
                });
            }
        }

        public async Task RemoveComment(int id)
        {
            string sql = $"DELETE FROM {CommentsLikesRepository.TableCreater.TableName} WHERE {CommentsLikesRepository.Columns.CommentId} = @id; DELETE FROM {TableName} where {Id} = @id;  ";
            await _connection.QueryAsync(sql, new { id = id });
        }
    }
}
