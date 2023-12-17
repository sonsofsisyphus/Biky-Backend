using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;
using Biky_Backend.Services.DTO;
using Biky_Backend.ActionFilters;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to social media post likes.
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILogger<LikeController> _logger;
        private readonly LikeService _likeService;

        public LikeController(ILogger<LikeController> logger, LikeService likeService)
        {
            _logger = logger;
            _likeService = likeService;
        }

        // Endpoint to add like.
        [HttpPost]
        [Route("Add")]
        [InjectUserId(typeof(LikeRequest), "UserID")]
        public IActionResult AddLike([FromBody] LikeRequest like)
        {
            try
            {
                _likeService.AddLike(like);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding like.");
                return BadRequest("Error adding like.");
            }
        }

        // Endpoint to remove a like.
        [HttpPost]
        [Route("Remove")]
        [InjectUserId(typeof(LikeRequest), "UserID")]
        public IActionResult RemoveLike([FromBody] LikeRequest like)
        {
            try
            {
                _likeService.RemoveLike(like);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing like.");
                return BadRequest("Error removing like.");
            }
        }

        // Endpoint to check if a like exists.
        [HttpGet]
        [Route("Exists")]
        public IActionResult LikeExists(LikeRequest like)
        {
            try
            {
                var result = _likeService.Exists(like);
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if like exists.");
                return BadRequest("Error checking if like exists.");
            }
        }
    }
}