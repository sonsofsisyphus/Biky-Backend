using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using Entities;
using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Biky_Backend.ActionFilters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to sale posts.
    [ApiController]
    [Route("[controller]")]
    public class SalePostController : ControllerBase
    {
        private readonly ILogger<SalePostController> _logger;
        private readonly SalePostService _salePostService;
        private readonly FeedService _feedService;

        public SalePostController(ILogger<SalePostController> logger, SalePostService salePostService, FeedService feedService)
        {
            _logger = logger;
            _salePostService = salePostService;
            _feedService = feedService;
        }

        // Endpoint to get a sale post by its unique ID.
        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID([FromQuery] Guid postID)
        {
            try
            {
                SalePost post = _salePostService.GetPostByPostID(postID);
                if (post != null)
                    return Ok(post);
                return NotFound("Post not found!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sale post by ID.");
                return BadRequest("Error getting sale post by ID.");
            }
        }

        // Endpoint to get sale posts by the author's user ID.
        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            try
            {
                List<SalePostSendRequest> posts = _feedService.GetSaleUser(authorID);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sale posts by user ID.");
                return BadRequest("Error getting sale posts by user ID.");
            }
        }

        // Endpoint to add a new sale post.
        [HttpPost]
        [Route("Add")]
        [InjectUserId(typeof(SalePostAddRequest), "AuthorID")]
        public IActionResult AddPost([FromBody] SalePostAddRequest addRequest)
        {
            try
            {
                var result = _salePostService.AddPost(addRequest);
                if (result != null)
                    return CreatedAtAction(nameof(GetPostByPostID), new { result }, result);
                return BadRequest("Post cannot be added");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding sale post.");
                return BadRequest("Error adding sale post.");
            }
        }

        // Endpoint to edit a sale post.
        [HttpPost]
        [Route("Edit")]
        public IActionResult EditPost([FromBody] SalePost salePost)
        {
            try
            {
                var result = _salePostService.UpdatePost(salePost);
                if (result != null)
                    return Ok(salePost.PostID);
                return BadRequest("Post cannot be updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing sale post.");
                return BadRequest("Error editing sale post.");
            }
        }

        // Endpoint to remove a sale post by its ID.
        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemovePost(Guid postID)
        {
            try
            {
                var result = _salePostService.RemovePost(postID);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing sale post.");
                return BadRequest("Error removing sale post.");
            }
        }

        // Endpoint to get all sale posts.
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllFeed()
        {
            try
            {
                var result = _feedService.GetSaleAll();
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

        // Endpoint to get sale posts from users the current user is following.
        [HttpGet]
        [Route("GetFollowings")]
        [Authorize]
        public IActionResult GetFollowingsFeed()
        {
            try
            {
                var userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = _feedService.GetSaleFollowings(userID);
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

        // Endpoint to get sale posts filtered by specific criteria.
        [HttpPost]
        [Route("GetFiltered")]
        public IActionResult GetFilteredFeed([FromBody] SaleFilter filters)
        {
            try
            {
                var result = _feedService.GetSaleFiltered(filters);
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
