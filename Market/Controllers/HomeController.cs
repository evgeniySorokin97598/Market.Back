using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

 
[Route("Home")]
public class HomeController : Controller
{
    private LoggerLib.Interfaces.ILogger  _logger;
    private IDataBaseManager _manager;
    
    public HomeController(LoggerLib.Interfaces.ILogger logger, IDataBaseManager manager)
    {
        _logger = logger;
        _manager = manager;
    }


    [HttpGet("GetHomePageData")]
    public async Task<IActionResult> GetHomePageData()
    {
        try
        {
            var Caregories = await _manager.CategoriesRepository.GetCategoriesAsync();
            return Ok(Caregories);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}