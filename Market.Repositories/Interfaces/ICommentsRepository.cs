using Market.Entities.Dto;
using Market.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        public Task AddAsync(AddCommentRequest request, UserInfo userInfo);
    }
}
