using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class NotificationAddRequest
    {
        [Required]
        public Guid ReceiverID { get; set; }

        [Required]
        public string Content { get; set; }

        public Notification ToNotification()
        {
            return new Notification
            {
                NotificationID = Guid.NewGuid(),
                ReceiverID  = ReceiverID,
                Content = Content,
                IsSeen = false
            };
        }
    }
}
