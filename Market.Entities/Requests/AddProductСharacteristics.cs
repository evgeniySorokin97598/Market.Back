using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Requests
{
    public class AddProductСharacteristics
    {
        public long ProductId { get; set; }
        public string NameCategory { get; set; }
        public List<string> Subcategories { get; set; } = new List<string>();
    }

    
}
