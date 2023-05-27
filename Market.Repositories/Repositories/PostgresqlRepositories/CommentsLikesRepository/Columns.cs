using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsLikesRepository
{
    public class Columns
    {
        public static string CommentId = "CommentId";
        /// <summary>
        /// id пользователя, который оценил комент
        /// </summary>
        public static string UserId = "LikeUserId";

        public static string Id = "likeid";
    }
}
