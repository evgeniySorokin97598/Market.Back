using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Requests
{
    public class AddCommentRequest
    {
        public string Dignity { get; set; }
        public string Comment { get; set; }
        public string Flaws { get; set; }    
        public int Stars { get; set; }
        public int ProductId { get; set; }
         
    }
}
