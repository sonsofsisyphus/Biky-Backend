using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Comment
    {
        [Key]
        public Guid CommentID { get; set; }

        [ForeignKey("User")]
        public Guid AuthorID { get; set; }

        [ForeignKey("Post")]
        public Guid PostID { get; set; }

        public virtual User Author { get; set; }

        public virtual SocialMediaPost Post { get; set; }

        public string Content { get; set; }

        public DateTime PostTime { get; set; }
    }
}