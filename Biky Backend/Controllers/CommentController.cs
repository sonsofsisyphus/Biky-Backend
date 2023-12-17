using Biky_Backend.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to comments.
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

        // Endpoint to add a new comment.
        [HttpPost]
        [Route("Add")]
        [InjectUserId(typeof(CommentAddRequest), "AuthorID")]
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

        // Endpoint to edit an existing comment.
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

        // Endpoint to retrieve a comment by its unique ID.
        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetCommentByID( Guid commentID)
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

        // Endpoint to retrieve comments associated with a specific post.
        [HttpGet]
        [Route("GetByPost")]
        public IActionResult GetCommentByPost(Guid postID)
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

        // Endpoint to remove a comment by its unique ID.
        [HttpDelete]
        [Route("Delete")]
        public IActionResult RemoveComment(Guid commentID)
        {
            try
            {
                _commentService.RemoveComment(commentID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing comment.");
                return BadRequest("Error removing comment.");
            }
        }
    }
}
