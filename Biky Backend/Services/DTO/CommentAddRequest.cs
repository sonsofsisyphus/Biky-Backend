using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class CommentAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public Guid PostID { get; set; }

        [Required]
        public string Content { get; set; } 
        public Comment ToComment()
        {
            return new Comment
            {
                CommentID = Guid.NewGuid(),
                AuthorID = AuthorID,
                PostID = PostID,
                Content = Content,
                PostTime = DateTime.Now
            };
        }
    }
}
