using Microsoft.AspNetCore.Mvc;
using Biky_Backend.Services;
using Entities;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        // Endpoint to add a new category to the system.
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

        // Endpoint to retrieve a list of categories.
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

        // Endpoint to retrieve the name of one category
        [HttpGet]
        [Route("GetName")]
        public IActionResult GetCategory(int id)
        {
            try
            {
                var result = _categoryService.GetCategoryName(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category.");
                return BadRequest("Error adding category.");
            }
        }
    }
}