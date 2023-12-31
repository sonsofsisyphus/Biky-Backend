﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace Biky_Backend.Controllers
{
    // This controller handles operations related to user notifications.
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

        // Endpoint to mark all notifications as seen for the current user.
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

        // Endpoint to get the number of unseen notifications for the current user.
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

        // Endpoint to get all notifications for the current user.
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

        // Endpoint to get all unseen notifications for the current user.
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
