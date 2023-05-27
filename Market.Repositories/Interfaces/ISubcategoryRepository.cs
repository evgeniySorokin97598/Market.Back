using Market.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface ISubcategoryRepository
    {
        public Task<int> AddAsync(SubCategory category);
    }
}
