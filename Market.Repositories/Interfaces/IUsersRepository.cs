using Market.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        public Task<long> AddAsync(UserInfo info);
        public Task<long> GetUser(UserInfo info);
    }
}
