using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities.Dto
{
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryIconUrl { get; set; }
        public List<SubCategory> SubCategories { get; set; }

    }
}
