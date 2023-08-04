using Dapper;
using DataBaseLib.Interfaces;
using Market.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Market.Repositories.Repositories.PostgresqlRepositories.СharacteristicsRepository.Columns;
using static Market.Repositories.Repositories.PostgresqlRepositories.СharacteristicsRepository.TableCreater;



namespace Market.Repositories.Repositories.PostgresqlRepositories.СharacteristicsRepository
{
    public class Repository : BaseRepository, IСharacteristicsRepository
    {
        public string TableName = TableCreater.TableName;
        public Repository(IDbProvider connection) : base(connection)
        {
        }

        public async Task Remove(int id)
        {
            string sql = $"DELETE FROM {TableName} WHERE {Id} = @id";
            await _connection.QueryAsync(sql, new { id = id });
        }

        public async Task<int> Add(string name, int type, string characteristic,int id)
        {

            int count = await Get(id);
            if (count == 0)
            {
                string insetChararistic = $" INSERT INTO {TableName} ({СharacteristicName},{TypeСharacteristicsId},{Сharacteristic}) VALUES(@Сharacteristic,@TypeId,@Text) returning {Id}";
                return (await _connection.QueryAsync<int>(insetChararistic, new
                {
                    Сharacteristic = name,
                    TypeId = type,
                    Text = characteristic
                })).FirstOrDefault();
            }
            else
            {
                await Update(id, name, characteristic);
                return id;
            }
        }
        public async Task<int> Get(int id)
        {
            string sql = $"SELECT count(*) from {TableName} WHERE {Id} = @id   ";
            return (await _connection.QueryAsync<int>(sql, new
            {
                id = id
            })).FirstOrDefault();
        }
        public async Task Update(int id, string name, string text)
        {
            string sql = $"UPDATE {TableName} SET {СharacteristicName} = @Сharacteristic,{Сharacteristic} = @Text where {Id} = @id ";
            await _connection.QueryAsync(sql, new
            {
                id = id,
                Сharacteristic = name,
                Text = text,
            });
        }
    }
}
