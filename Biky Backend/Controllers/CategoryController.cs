using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json;
using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Entities;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly CategoryService _categoryService;

        public CategoryController(ILogger<ImageController> logger, CategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            try
            {
                _categoryService.AddCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category.");
                return BadRequest("Error adding category.");
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(_categoryService.GetCategories());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category.");
                return BadRequest("Error adding category.");
            }
        }


    }
}