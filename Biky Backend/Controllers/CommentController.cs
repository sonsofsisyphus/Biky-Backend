using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly CommentService _commentService;

        public CommentController(ILogger<CommentController> logger, CommentService commentService)
        {
            _logger = logger;
            _commentService = commentService;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddComment([FromBody] CommentAddRequest comment)
        {
            try
            {
                var result = _commentService.AddComment(comment);
                if(result != null)
                    return CreatedAtAction(nameof(GetCommentByID), new { result }, result);
                return BadRequest("Cannot add comment");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment.");
                return BadRequest("Error adding comment.");
            }
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditComment([FromBody] CommentEditRequest comment)
        {
            try
            {
                _commentService.EditComment(comment);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing comment.");
                return BadRequest("Error editing comment.");
            }
        }

        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetCommentByID([FromQuery] Guid commentID)
        {
            try
            {
                var comment = _commentService.GetCommentByID(commentID);
                if (comment != null)
                    return Ok(comment);
                return NotFound("Comment not found!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comment by ID.");
                return BadRequest("Error getting comment by ID.");
            }
        }

        [HttpGet]
        [Route("GetByPost")]
        public IActionResult GetCommentByPost([FromQuery] Guid postID)
        {
            try
            {
                var comments = _commentService.GetCommentByPost(postID);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments by post ID.");
                return BadRequest("Error getting comments by post ID.");
            }
        }
    }
}