using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace Biky_Backend.Controllers
{
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

        [HttpGet]
        [Route("Add")]
        public IActionResult AddLike([FromQuery] Like like)
        {
            _likeService.AddLike(like);
            return Ok();
        }

        [HttpGet]
        [Route("Remove")]
        public IActionResult RemoveLike([FromQuery] Like like)
        {
            _likeService.RemoveLike(like);
            return Ok();
        }

        [HttpGet]
        [Route("Exists")]
        //Will be used in the frontend to determine if 
        public IActionResult LikeExists([FromQuery] Like like)
        {
            var result = _likeService.Exists(like);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

    }
}