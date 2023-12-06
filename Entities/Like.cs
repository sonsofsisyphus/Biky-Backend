using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Like
    {
        [Key]
        public Guid LikeID { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }

        [ForeignKey("SocialMediaPost")]
        public Guid PostID { get; set; }

        public virtual User User { get; set; }

        public virtual SocialMediaPost Post { get; set; }
    }
}