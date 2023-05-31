using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface ITypeСharacteristicsRepository
    {
        public Task<int> Add(string name, long productId);
    }
}
