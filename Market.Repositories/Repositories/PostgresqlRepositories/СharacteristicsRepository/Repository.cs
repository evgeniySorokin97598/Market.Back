using Dapper;
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
        public Repository(IDbConnection connection) : base(connection)
        {



        }

        public async Task<int> Add(string name, int type, string characteristic)
        {

            int id = await Get(name, characteristic, type);
            if (id == 0)
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
        public async Task<int> Get(string name, string text, int type)
        {
            string sql = $"SELECT {Id} from {TableName} WHERE {СharacteristicName} = @Сharacteristic AND {Сharacteristic} = @Text AND {TypeСharacteristicsId} = @type";
            return (await _connection.QueryAsync<int>(sql, new
            {
                Сharacteristic = name,
                Text = text,
                type = type
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
