using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public string Brend { get; set; }
        public long SubCategoryid { get; set; }

        public List<ProductCharacteristicType> TypesCharacteristics { get; set; } = new List<ProductCharacteristicType>();
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }
}
