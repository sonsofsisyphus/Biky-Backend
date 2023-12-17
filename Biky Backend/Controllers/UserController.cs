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
    // This controller handles operations related to user profiles and authentication.
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

        // Endpoint to get a user by their unique ID.
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUserByID(Guid userID)
        {
            var user = _userService.GetUserByID(userID);
            return Content(JsonConvert.SerializeObject(user), "application/json");
        }

        // Endpoint to get the profile photo of a user by their user ID.
        [HttpGet]
        [Route("GetUserPhoto")]
        public IActionResult GetUserPhoto(Guid userID)
        {
            var user = _userService.GetUserPhoto(userID);
            return Content(JsonConvert.SerializeObject(user), "application/json");
        }

        // Endpoint to get followers of a user by their user ID.
        [HttpGet]
        [Route("GetFollowers")]
        public IActionResult GetFollowersByID(Guid userID)
        {
            List<Guid> followers = _userService.GetFollowersByID(userID);
            return Content(JsonConvert.SerializeObject(followers), "application/json");
        }

        // Endpoint to get users that a user is following by their user ID.
        [HttpGet]
        [Route("GetFollowings")]
        public IActionResult GetFollowingsByID(Guid userID)
        {
            List<Guid> followings = _userService.GetFollowingsByID(userID);
            return Content(JsonConvert.SerializeObject(followings), "application/json");
        }

        // Endpoint to add a user to the current user's following list.
        [HttpGet]
        [Route("AddFollowing")]
        [InjectUserId(typeof(FollowRequest), "FollowerID")]
        public IActionResult AddFollowing([FromQuery]FollowRequest follow)
        {
            _userService.AddFollowing(follow);
            return Content("", "application/json");
        }

        // Endpoint to check if a user is followed by the current user.
        [HttpGet]
        [Route("CheckFollowing")]
        [InjectUserId(typeof(FollowRequest), "FollowerID")]
        public IActionResult CheckFollowing([FromQuery] FollowRequest follow)
        {
            return Ok(_userService.CheckFollowing(follow));
        }

        // Endpoint to remove a user from the current user's following list.
        [HttpDelete]
        [Route("DeleteFollowing")]
        [InjectUserId(typeof(FollowRequest), "FollowerID")]
        public IActionResult DeleteFollowing([FromQuery] FollowRequest follow)
        {
            _userService.RemoveFollowing(follow);
            return Content("", "application/json");
        }

        // Endpoint for user login.
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
            var token = new UserLoginSendRequest() { Token = _jwtProvider.Generate(user), UserID = user.UserID };

            // Return it
            return Ok(token);
        }

        // Endpoint for user registration.
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

        // Endpoint to get all users.
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        // Endpoint to get a user's profile by their user ID.
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

        // Endpoint to update a user's profile.
        [HttpGet]
        [Route("UpdateProfile")]
        [InjectUserId(typeof(ProfileEditRequest), "userID")]
        public IActionResult UpdateProfile([FromQuery] ProfileEditRequest profile)
        {
            try
            {
                _userService.UpdateProfile(profile);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing profile.");
                return BadRequest("Error editing profile.");
            }
        }

        // Endpoint to search for users by their nickname.
        [HttpGet]
        [Route("SearchUser")]
        public IActionResult SearchUsersByNickname(string contains)
        {
            try
            {
                var result = _userService.searchUsersByNickname(contains);
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users.");
                return BadRequest("Error getting users.");
            }
        }
    }
}