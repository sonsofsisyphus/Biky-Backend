using Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biky_Backend.Services.DTO
{
    public class MesssageAddRequest
    {
        [Required]
        public Guid SenderID { get; set; }


        [Required]
        public Guid ReceiverID { get; set; }

        [Required]
        public Guid ChatID { get; set; }

        [Required]
        public string Content { get; set; }

        public Message ToMessage()
        {
            return new Message()
            {
                MessageID = Guid.NewGuid(),
                SenderID = SenderID,
                ReceiverID = ReceiverID,
                ChatID = ChatID,
                Content = Content,
                DateTime = DateTime.Now,
            };
        }
    }
}
