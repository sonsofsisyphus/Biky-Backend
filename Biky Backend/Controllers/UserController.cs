using Microsoft.AspNetCore.Mvc;
using Services;
using Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Services.DTO;
using Services.Authentication;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly JwtProvider _jwtProvider;
        
        public UserController(ILogger<UserController> logger, UserService userService, JwtProvider jwtProvider)
        {
            _logger = logger;
            _userService = userService;
            _jwtProvider = jwtProvider;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserLoginRequest login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }

            // Check if user entered exists and valid
            if (login.Nickname != "mahmut")
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Nickname does not exist!"
                    }
                });
            }

            var user = new User()
            {
                UserID = Guid.Parse("c71ae2ef-dcfc-419c-a6c5-5858716cb5bd"),
                UniversityID = "123456",
                Nickname = login.Nickname,
                Email = login.Password + "@example.com",
            };

            // Generate Jwt
            var token = _jwtProvider.Generate(user);

            // Return it
            return Ok(token);
        }

        [Authorize(Roles = "Normal")]
        [HttpGet]
        public IActionResult LoginTest() 
        {
            return Ok();
        }
    }
}