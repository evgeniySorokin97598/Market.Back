using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Requests
{
    public class GetProductsRequest
    {
        /// <summary>
        /// id  запрашиваемых товаров
        /// </summary>
        public List<long> Id= new List<long>();
    }
}
