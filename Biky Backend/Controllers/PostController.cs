using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly PostService _postService;
        public PostController(ILogger<UserController> logger, PostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID([FromQuery] Guid postID)
        {
            Post post = _postService.GetPostByPostID(postID);
            return Content(JsonConvert.SerializeObject(post), "application/json");
        }

    }
}