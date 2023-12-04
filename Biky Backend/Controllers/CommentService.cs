using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
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
            Guid commentID = _commentService.AddComment(comment);
            return Content(JsonConvert.SerializeObject(commentID), "application/json");
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult EditComment([FromBody] CommentEditRequest comment)
        {
            _commentService.EditComment(comment);
            return Ok();
        }

        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetCommentByID([FromQuery] Guid commentID)
        {
            var comment = _commentService.GetCommentByID(commentID);
            return Content(JsonConvert.SerializeObject(comment), "application/json");
        }

        [HttpGet]
        [Route("GetByPost")]
        public IActionResult GetCommentByPost([FromQuery] Guid postID)
        {
            var comments = _commentService.GetCommentByPost(postID);
            return Content(JsonConvert.SerializeObject(comments), "application/json");
        }

    }
}