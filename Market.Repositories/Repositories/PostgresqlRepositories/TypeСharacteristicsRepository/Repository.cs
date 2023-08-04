using Dapper;
using DataBaseLib.Interfaces;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Market.Repositories.Repositories.PostgresqlRepositories.TypeСharacteristicsRepository.Columns;

namespace Market.Repositories.Repositories.PostgresqlRepositories.TypeСharacteristicsRepository
{
    public class Repository : BaseRepository, ITypeСharacteristicsRepository
    {
        public string TableName = TableCreater.TableName;
        public Repository(IDbProvider connection) : base(connection)
        {



        }

        public async Task<int> Add(string name, long productId)
        {
            int id = await GetByName(name, productId);
            if (id == 0)
            {
                string sql = $"INSERT INTO {TableName} ({TypeСharacteristicsName},{ProductId}) VALUES(@Name,@ProductId) returning id";
                return (await _connection.QueryAsync<int>(sql, new
                {
                    Name = name,
                    ProductId = productId
                })).FirstOrDefault();
            }
            else return id;
        }
        public async Task<int> GetByName(string name, long productId) 
        {
            string sql = $"SELECT {Id} from {TableName} WHERE {TypeСharacteristicsName} = @Name AND {ProductId} = @ProductId ";
            return (await _connection.QueryAsync<int>(sql, new
            {
                Name = name,
                ProductId = productId
            })).FirstOrDefault();
        } 
    }
}
