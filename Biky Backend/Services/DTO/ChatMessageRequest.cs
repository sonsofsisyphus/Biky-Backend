using Entities;
using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ChatMessageRequest
    {
        [Required] public Guid MessageID { get; set; }
        [Required] public Guid SenderID { get; set; }
        [Required] public Guid ReceiverID { get; set; }
        [Required] public string Content { get; set; }
        [Required] public DateTime DateTime {  get; set; }

        public static ChatMessageRequest ToChatMessageRequest(ChatMessage message)
        {
            return new ChatMessageRequest
            {
                MessageID = message.MessageID,
                SenderID = message.SenderID,
                ReceiverID = message.ReceiverID, 
                Content = message.Content,
                DateTime = message.DateTime,
            };
        }
    }
}
