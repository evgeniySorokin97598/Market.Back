using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLib.Interfaces
{
    public interface IDbProvider
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string query, object obj = null);
        public Task<IEnumerable<dynamic>> QueryAsync(string query, object obj = null);
    }
}
