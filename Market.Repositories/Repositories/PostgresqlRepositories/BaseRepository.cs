using DataBaseLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Repositories.PostgresqlRepositories
{
    public abstract class BaseRepository
    {
        protected IDbProvider _connection;
        public BaseRepository(IDbProvider connection)
        {
            _connection = connection;
        }
    }
}
