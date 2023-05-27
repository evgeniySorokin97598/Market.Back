using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Dto
{
    public class CommentDto
    {
        public long CommentId { get; set; }
        public string Dignity { get; set; }
        public string Comment { get; set; }
        public string Flaws { get; set; }
        public string UserName { get; set; }
        public int Stars { get; set; }
        public int CountLikes { get; set; }
    }
}
