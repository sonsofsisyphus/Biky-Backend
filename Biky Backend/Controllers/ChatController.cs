using Biky_Backend.Services.DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("chat")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly DBConnector _dbContext;

    public ChatController(DBConnector dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("messages")]
    public IActionResult GetMessages([FromBody] ChatMessageSendRequest msg)
    {
        var messages = _dbContext.ChatMessages
            .Where(m => (
                            m.ReceiverID == msg.recieverId && m.SenderID == msg.senderId
                            ) || (
                            m.ReceiverID == msg.senderId && m.SenderID == msg.recieverId
                            )
                )
            .OrderBy(m => m.DateTime)
            .ToList();
        return Ok(messages);
    }

    [HttpPost]
    [Route("send")]
    public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] ChatMessageAddRequest message)
    {
        if (message == null)
        {
            return BadRequest();
        }

        var msg = message.ToMsg();
        _dbContext.ChatMessages.Add(msg);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMessages),msg);
    }
}