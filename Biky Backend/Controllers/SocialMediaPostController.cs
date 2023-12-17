using Biky_Backend.ActionFilters;
using Biky_Backend.Services;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Security.Claims;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to social media posts and feeds.
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

        // Endpoint to get a social media post by its unique ID.
        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID(Guid postID)
        {
            var post = _socialMediaPostService.GetPostByPostID(postID);
            if (post != null)
                return Ok(post);
            return NotFound("Post not found!");
        }

        // Endpoint to get social media posts by the author's user ID.
        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            try
            {
                var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                List<SocialMediaPostSendRequest> posts = _feedService.GetSocialMediaUser(userID,
                    authorID);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting feed.");
                return BadRequest();
            }
        }

        // Endpoint to add a new social media post.
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

        // Endpoint to edit a social media post.
        [HttpPost]
        [Route("Edit")]
        public IActionResult EditPost([FromBody] SocialMediaPost socialMediaPost)
        {
            var result = _socialMediaPostService.UpdatePost(socialMediaPost);
            if (result != null)
                return Ok(result);
            return BadRequest("Post cannot be updated");
        }

        // Endpoint to remove a social media post by its ID.
        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemovePost(Guid postID)
        {
            var result = _socialMediaPostService.RemovePost(postID);
            if (result)
                return NoContent();
            return NotFound();
        }

        // Endpoint to get all social media posts in the feed.
        [HttpGet]
        [Route("GetAllFeed")]
        [Authorize]
        public IActionResult GetAllFeed()
        {
            var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _feedService.GetSocialMediaAll(userID);
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        // Endpoint to get social media posts from users the current user is following.
        [HttpGet]
        [Route("GetFollowingsFeed")]
        [Authorize]
        public IActionResult GetFollowingsFeed()
        {
            var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _feedService.GetSocialMediaFollowings(userID);
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        // Endpoint to get social media posts for guests(unauthenticated users).
        [HttpGet]
        [Route("GetGuestFeed")]
        [AllowAnonymous]
        public IActionResult GetGuestFeed()
        { 
            var result = _feedService.GetSocialMediaGuest();
            if (result != null)
                return Ok(result);
            return BadRequest("Feed couldn't be retrieved");
        }

        // Endpoint to get social media posts filtered by content.
        [HttpGet]
        [Route("GetFeedByContent")]
        public IActionResult GetFeedByContent(string contains)
        {
            try
            {
                var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = _feedService.GetSocialMediaByContent(contains, userID);
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
