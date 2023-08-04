using Dapper;
using DataBaseLib.Interfaces;
using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Market.Repositories.Repositories.PostgresqlRepositories.SubcategoryRepository.Columns;
namespace Market.Repositories.Repositories.PostgresqlRepositories.SubcategoryRepository
{
    public class Repository : BaseRepository, ISubcategoryRepository
    {

        private string TableName = TableCreater.TableName;

        
        public Repository(IDbProvider connection):base(connection) 
        {
             
        }
        public async Task<int> AddAsync(SubCategory category)
        {
            string checkSql = $"SELECT {IdColumnName} FROM {TableName} WHERE {NameColumnName} = @name";
            var check = await _connection.QueryAsync<int>(checkSql, new { name = category.SubCategoryName });
            if (!check.Any())
            {
                string sql = $"INSERT INTO {TableName} " +
                    $"({NameColumnName},{UrlIconcColumnName},{CategoryIdColumnName}) " +
                    $"VALUES (@Name,@UrlIcon,@CategoryId) " +
                    $"returning {IdColumnName}";

                var result = (await _connection.QueryAsync<int>(sql, new
                {
                    Name = category.SubCategoryName,
                    UrlIcon = category.SubCategoryUrlIcon,
                    category.CategoryId
                })).FirstOrDefault();
                return result;
            }
            else return check.FirstOrDefault();

        }
    }
}
