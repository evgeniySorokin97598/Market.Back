using Market.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface ICommentsLikesRepository
    {
        public Task LikeComment(long commentId, UserInfo user);
    }
}
