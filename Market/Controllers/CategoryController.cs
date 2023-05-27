using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{

    [Route("Categories")]
    public class CategoryController : Controller
    {
        

        private ICategoriesRepository _repository;
        public CategoryController(IDataBaseManager manager)
        {
            _repository = manager.CategoriesRepository;

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
