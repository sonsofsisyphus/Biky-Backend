using Biky_Backend.ActionFilters;
using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Security.Claims;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SocialMediaPostController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly FeedService _feedService;

        public SocialMediaPostController(ILogger<UserController> logger, SocialMediaPostService socialMediaPostService, FeedService feedService)
        {
            _logger = logger;
            _socialMediaPostService = socialMediaPostService;
            _feedService = feedService;
        }

        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID(Guid postID)
        {
            var post = _socialMediaPostService.GetPostByPostID(postID);
            if (post != null)
                return Ok(post);
            return NotFound("Post not found!");
        }

        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID( Guid authorID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetPostByUserID(authorID);
            return Ok(posts);
        }

        [HttpPost]
        [Route("Add")]
        [InjectUserId(typeof(SocialMediaPostAddRequest), "AuthorID")]
        public IActionResult AddPost([FromBody] SocialMediaPostAddRequest addRequest)
        {
            var result = _socialMediaPostService.AddPost(addRequest);
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
        public IActionResult RemovePost(Guid postID)
        {
            var result = _socialMediaPostService.RemovePost(postID);
            if (result)
                return NoContent();
            return NotFound();
        }

        [HttpGet]
        [Route("GetAllFeed")]
        public IActionResult GetAllFeed()
        {
            var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _feedService.GetSocialMediaAll(userID);
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        [HttpGet]
        [Route("GetFollowingsFeed")]
        public IActionResult GetFollowingsFeed()
        {
            var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _feedService.GetSocialMediaFollowings(userID);
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        [HttpGet]
        [Route("GetGuestFeed")]
        public IActionResult GetGuestFeed()
        { 
            var result = _feedService.GetSocialMediaGuest();
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        [HttpGet]
        [Route("GetFeedByContent")]
        public IActionResult GetFeedByContent(string contains)
        {
            try
            {
                var result = _feedService.GetSocialMediaByContent(contains);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting feed.");
                return BadRequest("Error getting feed.");
            }
        }
    }
}
