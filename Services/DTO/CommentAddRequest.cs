using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
