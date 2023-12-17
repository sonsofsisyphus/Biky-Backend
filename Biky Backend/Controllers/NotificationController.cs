using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Security.Claims;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<LikeController> _logger;
        private readonly NotificationService _notificationService;

        public NotificationController(ILogger<LikeController> logger, NotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("SetAllSeen")]
        public IActionResult SetAllSeen()
        {
            try
            {
                var receiverID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _notificationService.SetAllSeen(receiverID);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUnseen")]
        public IActionResult GetUnseenNotificationNumber()
        {
            try
            {
                var receiverID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var unseenNotificationNo = _notificationService.GetUnseenNotificationNumber(receiverID);
                return Ok(unseenNotificationNo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllNotifications()
        {
            try
            {
                var receiverID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var notifications = _notificationService.GetAllNotifications(receiverID);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllUnseen")]
        public IActionResult GetAllUnseenNotifications()
        {
            try
            {
                var receiverID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var notifications = _notificationService.GetAllUnseenNotifications(receiverID);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
