using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly ImageService _imageService;
        public ImageController(ILogger<UserController> logger, ImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var result = await _imageService.UploadFile(file);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }


    }
}