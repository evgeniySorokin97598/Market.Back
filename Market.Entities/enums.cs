using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Entities
{
    public enum OrderBy
    {
        /// <summary>
        /// сортировака по цене (от меньшей к большей)
        /// </summary>
        Price,
        /// <summary>
        /// сортировака по цене (от большей  к меньшей)
        /// </summary>
        DescPrice,
        /// <summary>
        /// без сортировки
        /// </summary>
        None
    }
}
