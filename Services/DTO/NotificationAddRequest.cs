using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class NotificationAddRequest
    {
        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public Guid ReceiverID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        public Guid ContentID { get; set; }
        public Notification ToNotification()
        {
            return new Notification
            {
                NotificationID = Guid.NewGuid(),
                NotificationType = NotificationType,
                ReceiverID  = ReceiverID,
                UserID = UserID,
                ContentID = ContentID,
                IsSeen = false
            };
        }
    }
}
