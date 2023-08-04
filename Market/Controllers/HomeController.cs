using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

 
[Route("Home")]
public class HomeController : Controller
{
    private LoggerLib.Interfaces.ILogger  _logger;
    ICategoriesRepository _repository;


    public HomeController(LoggerLib.Interfaces.ILogger logger, ICategoriesRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }


    [HttpGet("GetHomePageData")]
    public async Task<IActionResult> GetHomePageData()
    {
        try
        {
            var Caregories = await _repository.GetCategoriesAsync();
            return Ok(Caregories);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}