using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface IСharacteristicsRepository
    {
        Task<int> Add(string name, int type, string characteristic);
    }
}
