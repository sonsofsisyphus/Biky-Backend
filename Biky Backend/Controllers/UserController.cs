using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        public UserController(ILogger<UserController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUserByID([FromQuery] Guid userID)
        {
            User user = _userService.GetUserByID(userID);
            return Content(JsonConvert.SerializeObject(user), "application/json");
        }

        [HttpGet]
        [Route("GetFollowers")]
        public IActionResult GetFollowersByID([FromQuery] Guid userID)
        {
            List<Guid> followers = _userService.GetFollowersByID(userID);
            return Content(JsonConvert.SerializeObject(followers), "application/json");
        }

        [HttpGet]
        [Route("GetFollowings")]
        public IActionResult GetFollowingsByID([FromQuery] Guid userID)
        {
            List<Guid> followings = _userService.GetFollowingsByID(userID);
            return Content(JsonConvert.SerializeObject(followings), "application/json");
        }

        [HttpGet]
        [Route("AddFollowing")]
        public IActionResult addFollowing([FromQuery] Following following)
        {
            _userService.AddFollowing(following);
            return Content("", "application/json");
        }

    }
}