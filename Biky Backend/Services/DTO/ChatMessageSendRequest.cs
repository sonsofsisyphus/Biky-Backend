using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ChatMessageSendRequest
    {
        [Required] public Guid senderId { get; set; }
        [Required] public Guid recieverId { get; set; }
    }
}