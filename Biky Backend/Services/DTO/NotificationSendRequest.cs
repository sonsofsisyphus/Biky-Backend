using Entities;
using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class NotificationSendRequest
    {
        [Key]
        public Guid NotificationID { get; set; }

        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public static NotificationSendRequest ToNotificationSendRequest(Notification notification) 
        {
            return new NotificationSendRequest
            {
                NotificationID = notification.NotificationID,
                Content = notification.Content,
                IsSeen = notification.IsSeen,
            };
        }
    }
}
