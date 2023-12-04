using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum NotificationType {
        FOLLOW,
        LIKE,
        FOLLOW_POST, //An account you are following posted a new social media post.
        //Is this necessary? Evaluate. (No functionality added for now, except remove func.)
        COMMENT

    };
    public class Notification
    {   
        [Required]
        public Guid NotificationID { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        public Guid ReceiverID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        public Guid ContentID { get; set; }
        //Used for LIKE, FOLLOW_POST as ID of a post.
        //For COMMENT it is ID of a comment.

        [Required]
        public bool IsSeen { get; set; }
    }
}
