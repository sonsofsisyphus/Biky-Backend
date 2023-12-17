using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public enum NotificationType
    {
        LIKE,
        COMMENT,
        FOLLOW
    }

    public class Notification
    {
        [Key]
        public Guid NotificationID { get; set; }

        [ForeignKey("User")]
        public Guid ReceiverID { get; set; }

        public virtual User Receiver { get; set; }

        public string Content { get; set; }

        public bool IsSeen { get; set; }
    }
}