using Market.Entities;
using Market.Entities.Dto;
using Market.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        /// <summary>
        /// получение товаров по категории без подробных характеристик
        /// </summary>
        /// <param name="categyName"></param>
        /// <returns></returns>
        public Task<IEnumerable<ProductDto>> GetProductsByCategory(string categyName, OrderBy order);
        /// <summary>
        /// добавление нового товара
        /// </summary>
        /// <param name="product"></param>
        /// <returns>возвращает id добавленного товара</returns>
        public Task<int> AddAsync(ProductDto product);

        /// <summary>
        /// добавление характеристик к товару
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        public Task AddCharectiristic(ProductCharacteristicType  characteristic);

        /// <summary>
        /// получение всех инфы о товаре
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ProductDto>GetProductById(long id);

        public Task<IEnumerable<ProductDto>> GetProducts(GetProductsRequest request);
    }
}
