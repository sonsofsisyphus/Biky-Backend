using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageService _imageService;

        public ImageController(ILogger<ImageController> logger, ImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var result = await _imageService.UploadFile(file);
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image.");
                return BadRequest("Error uploading image.");
            }
        }
    }
}