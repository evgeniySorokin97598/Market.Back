﻿using Market.Entities;
using Market.Entities.Dto;
using Market.Entities.Requests;
using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private LoggerLib.Interfaces.ILogger _logger;
         
        private IProductsRepository _repository;

        public ProductsController(LoggerLib.Interfaces.ILogger logger, IProductsRepository repositoy)
        {
            _logger = logger;

            _repository = repositoy;
        }

        [HttpPost("GetProducts")]
        public async Task<IActionResult> GetProducts([FromBody] List<long> longs)
        {

            try
            {
                longs = longs.Distinct().ToList();
                var products = await _repository.GetProducts(new GetProductsRequest()
                {
                    Id = longs
                });
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetProductsByCategory/{category}/{filter}")]
        public async Task<IActionResult> GetProductsByCategory(string category, OrderBy filter = OrderBy.None)
        {
            try
            {
                var result = await _repository.GetProductsByCategory(category, filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при получении товаров по категории {category}");
                return StatusCode(500);
            }
        }



        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
        {
            try
            {
                await _repository.AddAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при добавлении нового товара {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var result = await _repository.GetProductById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddCharectiristics")]
        public async Task<IActionResult> AddCharectiristics([FromBody] ProductCharacteristicType request)
        {

            try
            {
                await _repository.AddCharectiristic(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


    }
}
