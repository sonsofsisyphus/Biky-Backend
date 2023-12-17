using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to image uploads.
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

        // Endpoint to upload an image file.
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                // Call the ImageService to upload the image file.
                var result = await _imageService.UploadFile(file);

                // Serialize the result and return it as JSON.
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