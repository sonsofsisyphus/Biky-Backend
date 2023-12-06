using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Like
    {
        [ForeignKey("User")]
        public Guid UserID { get; set; }

        [ForeignKey("SocialMediaPost")]
        public Guid PostID { get; set; }

        public virtual User User { get; set; }

        public virtual SocialMediaPost Post { get; set; }
    }
}