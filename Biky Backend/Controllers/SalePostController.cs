using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using Entities;
using Biky_Backend.Services;

namespace Biky_Backend.Controllers
{
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

        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            try
            {
                List<SalePost> posts = _salePostService.GetPostsByUserID(authorID);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sale posts by user ID.");
                return BadRequest("Error getting sale posts by user ID.");
            }
        }

        [HttpPost]
        [Route("Add")]
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

        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemovePost([FromQuery] Guid postID)
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
    }
}
