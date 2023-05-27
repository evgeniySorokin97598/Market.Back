using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Dto
{
    public class ProductCharacteristicType
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public List<Charastitic> Charastitics { get; set; } = new List<Charastitic>();
    }
}
