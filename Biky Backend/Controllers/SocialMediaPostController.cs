using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

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
            if (post != null)
                return Ok(post);
            return NotFound("Post not found!");
        }

        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetPostByUserID(authorID);
            return Ok(posts);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddPost([FromBody] SocialMediaPostAddRequest addRequest)
        {
            var result = _socialMediaPostService.AddPost(addRequest.ToSocialMediaPost());
            if (result != null)
                return CreatedAtAction(nameof(GetPostByPostID), new { result }, result);
            return BadRequest("Post cannot be added");
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditPost([FromBody] SocialMediaPost socialMediaPost)
        {
            var result = _socialMediaPostService.UpdatePost(socialMediaPost);
            if (result != null)
                return Ok(result);
            return BadRequest("Post cannot be updated");
        }

        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemovePost([FromQuery] Guid postID)
        {
            var result = _socialMediaPostService.RemovePost(postID);
            if (result)
                return NoContent();
            return NotFound();
        }
    }
}
