using System.ComponentModel.DataAnnotations;
using Entities;

namespace Biky_Backend.Services.DTO
{

    public class ChatMessageAddRequest
    {
        [Required] public Guid SenderID { get; set; }

        [Required] public Guid ReceiverID { get; set; }

        [Required] public string Content { get; set; }

        public ChatMessage ToMsg()
        {
            return new ChatMessage()
            {
                MessageID = Guid.NewGuid(),
                SenderID = SenderID,
                ReceiverID = ReceiverID,
                Content = Content,
                DateTime = DateTime.UtcNow,
            };
        }
    }
}