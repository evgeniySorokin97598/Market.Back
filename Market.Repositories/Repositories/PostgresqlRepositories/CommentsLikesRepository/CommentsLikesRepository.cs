using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository.TableCreater;
using static Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository.Columns;
using Dapper;
using Market.Repositories.Interfaces;
using Market.Entities.Dto;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository
{
    public class Repository : BaseRepository, ICommentsLikesRepository
    {
        private IUsersRepository _usersRepository;
        public Repository(NpgsqlConnection connection, IUsersRepository usersRepository) : base(connection)
        {
            _usersRepository = usersRepository;
        }
        public async Task LikeComment(long commentId, UserInfo info)
        {
            try
            {
                await _usersRepository.AddAsync(info);
            }
            catch (Exception ex)
            {

            }
            /// проверка ну дубль лайка
            var check = await _connection.QueryAsync<int>($"SELECT COUNT(*) FROM {TableName}  WHERE {UserId} = @userId AND {CommentId} = @commentId", new
            {
                commentId = commentId,
                userId = info.Id
            });
            if (check.FirstOrDefault() == 0)
            {
                string sql = $"INSERT INTO {TableName} ({CommentId},{UserId}) VALUES (@commentId,@userId)";
                await _connection.QueryAsync(sql, new
                {
                    commentId = commentId,
                    userId = info.Id
                });
            }


        }
    }
}
