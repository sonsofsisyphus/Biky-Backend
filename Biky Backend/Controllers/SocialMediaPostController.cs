using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;
using Services.DTO;
using Microsoft.Extensions.Hosting;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SocialMediaPostController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly SocialMediaPostService _socialMediaPostService;
        public SocialMediaPostController(ILogger<UserController> logger, SocialMediaPostService socialMediaPostService)
        {
            _logger = logger;
            _socialMediaPostService = socialMediaPostService;
        }

        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID([FromQuery] Guid postID)
        {
            var post = _socialMediaPostService.GetPostByPostID(postID);
            return Content(JsonConvert.SerializeObject(post), "application/json");
        }

        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetPostByUserID(authorID);
            return Content(JsonConvert.SerializeObject(posts), "application/json");
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddPost([FromBody] SocialMediaPostAddRequest socialMediaPost)
        {
            Guid postID = _socialMediaPostService.AddPost(socialMediaPost);
            return Content(JsonConvert.SerializeObject(postID), "application/json");
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditPost([FromBody] SocialMediaPost socialMediaPost)
        {
            _socialMediaPostService.UpdatePost(socialMediaPost);
            return Content(JsonConvert.SerializeObject(socialMediaPost.PostID), "application/json");
        }

        [HttpGet]
        [Route("Remove")]
        public IActionResult RemovePost([FromQuery] Guid postID)
        {
            _socialMediaPostService.RemovePost(postID);
            return Content(JsonConvert.SerializeObject(postID), "application/json");
        }

    }
}