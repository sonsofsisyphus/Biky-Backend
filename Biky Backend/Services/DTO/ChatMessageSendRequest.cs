using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ChatMessageSendRequest
    {
        [Required] public Guid SenderID { get; set; }
        [Required] public Guid ReceiverID { get; set; }
    }
}