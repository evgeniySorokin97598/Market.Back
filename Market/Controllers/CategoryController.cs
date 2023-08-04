using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{

    [Route("Categories")]
    public class CategoryController : Controller
    {
        

        private ICategoriesRepository _repository;
        public CategoryController(ICategoriesRepository repository)
        {
            _repository = repository;

        }

        [HttpGet("GetSubCategories/{category}")]
        public async Task<IActionResult> GetSubCategories(string category)
        {
            try
            {
                return Ok(await _repository.GetSubCategories(category));
            }
            catch
            {
                return StatusCode(500);
            }

        }
    }
}
