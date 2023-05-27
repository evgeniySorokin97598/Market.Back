using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Repositories.PostgresqlRepositories.CommentsRepository
{
    public static class Columns
    {
         

        public static string Id = "CommentId";

        /// <summary>
        /// колонка с достоинствами товара
        /// </summary>
        public static string DignityColumnName = "Dignity";

        /// <summary>
        /// колонка с недостатками товара
        /// </summary>
        public static string Flaws = "Flaws";

        /// <summary>
        /// колонка с текстом комментария
        /// </summary>
        public static string Comment = "Comment";

        public static string ProductId = "ProductId";
        public static string UserIdCol = "UserId";
        public static string Stars = "Stars";
        public static string Likes = "likes";
    }
}
