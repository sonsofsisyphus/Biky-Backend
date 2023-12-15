using Microsoft.AspNetCore.Mvc;
using Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Services.DTO;
using Services.Authentication;
using Biky_Backend.Services.DTO;
using Biky_Backend.ActionFilters;

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
            var user = _userService.GetUserByID(userID);
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
        [InjectUserId(typeof(FollowRequest), "FollowerID")]
        public IActionResult AddFollowing([FromQuery] FollowRequest follow)
        {
            _userService.AddFollowing(follow);
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

            var user = _userService.GetUserByNickname(login.Nickname);

            if (user == null || !_userService.ValidatePassword(user.UserID, login.Password))
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid nickname or password"
                    }
                });
            }

            // Generate Jwt
            var token = _jwtProvider.Generate(user);

            // Return it
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserRegisterRequest register)
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

            var registrationResult = _userService.Register(register);

            if (!registrationResult)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Registration failed"
                    }
                });
            }

            return Ok(new AuthResult() { Result = true });
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet]
        [Route("GetProfile")]
        public IActionResult GetProfile(Guid userID)
        {
            try
            {
                var result = _userService.GetProfileByID(userID);
                if (result != null) return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accessing profile.");
                return BadRequest("Error accessing profile.");
            }
        }
                 
    }
}
