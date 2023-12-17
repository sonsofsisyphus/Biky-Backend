using Services.DTO;
using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ChatHistorySendRequest
    {
        [Required] 
        public UserSendRequest User { get; set; }

        [Required] 
        public ChatMessageRequest LastMessage { get; set; }

        public ChatHistorySendRequest(UserSendRequest user, ChatMessageRequest lastMessage)
        {
            User = user;
            LastMessage = lastMessage;
        }
    }
}
