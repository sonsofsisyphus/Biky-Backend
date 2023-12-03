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
    public class SalePostController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly SalePostService _salePostService;
        public SalePostController(ILogger<UserController> logger, SalePostService salePostService)
        {
            _logger = logger;
            _salePostService = salePostService;
        }

        [HttpGet]
        [Route("GetPost")]
        public IActionResult GetPostByPostID([FromQuery] Guid postID)
        {
            SalePost post = _salePostService.GetPostByPostID(postID);
            return Content(JsonConvert.SerializeObject(post), "application/json");
        }

        [HttpGet]
        [Route("GetPostByUser")]
        public IActionResult GetPostByAuthorID([FromQuery] Guid authorID)
        {
            List<SalePost> posts = _salePostService.GetPostByUserID(authorID);
            return Content(JsonConvert.SerializeObject(posts), "application/json");
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddPost([FromBody] SalePostAddRequest salePost)
        {
            Guid postID = _salePostService.AddPost(salePost);
            return Content(JsonConvert.SerializeObject(postID), "application/json");
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult EditPost([FromBody] SalePost salePost)
        {
            _salePostService.UpdatePost(salePost);
            return Content(JsonConvert.SerializeObject(salePost.PostID), "application/json");
        }

        [HttpGet]
        [Route("Remove")]
        public IActionResult RemovePost([FromQuery] Guid postID)
        {
            _salePostService.RemovePost(postID);
            return Content(JsonConvert.SerializeObject(postID), "application/json");
        }

    }
}