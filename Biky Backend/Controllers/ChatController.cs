using Biky_Backend.ActionFilters;
using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

// This controller handles operations related to chat functionality.
[Route("Chat")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly DBConnector _dbContext;
    private readonly ChatService _chatService;

    public ChatController(DBConnector dbContext, ChatService chatService)
    {
        _dbContext = dbContext;
        _chatService = chatService;
    }

    // Endpoint to retrieve chat messages for a specified user.
    [HttpPost]
    [Route("GetMessages")]
    [InjectUserId(typeof(ChatMessageSendRequest), "ReceiverID")]
    public IActionResult GetMessages([FromBody] ChatMessageSendRequest msg)
    {
        var messages = _chatService.GetMessages(msg);
        return Ok(messages);
    }

    // Endpoint to send a chat message.
    [HttpPost]
    [Route("Send")]
    [InjectUserId(typeof(ChatMessageAddRequest), "SenderID")]
    public IActionResult SendMessage([FromBody] ChatMessageAddRequest message)
    {
        // Check if the provided message is valid.
        if (message == null && message.Content == "")
            return BadRequest();

        var msg = _chatService.SendMessage(message);

        if (msg != null)
            return CreatedAtAction(nameof(GetMessages), msg);
        else
            return BadRequest("Message cannot be sent");
    }

    // Endpoint to open a chat with a specified receiver.
    [HttpGet]
    [Route("OpenChat")]
    [InjectUserId(typeof(ChatMessageSendRequest), "SenderID")]
    public IActionResult OpenChat(Guid receiverID)
    {
        var currentUserID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        _chatService.OpenChat(currentUserID, receiverID);
        return Ok();
    }

    // Endpoint to retrieve all chat histories for the current user.
    [HttpGet]
    [Route("GetAllChats")]
    public IActionResult GetAllChats()
    {
        var currentUserID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var chatHistory = _chatService.GetAllChats(currentUserID);

        return Ok(chatHistory);
    }
}