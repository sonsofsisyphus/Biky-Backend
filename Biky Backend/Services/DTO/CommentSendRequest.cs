using Entities;
using Services.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biky_Backend.Services.DTO
{
    public class CommentSendRequest
    {
        public Guid CommentID { get; set; }
        public Guid AuthorID { get; set; }
        public Guid PostID { get; set; }
        public UserSendRequest Author { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }

        public static CommentSendRequest ToCommentSendRequest(Comment c)
        {
            return new CommentSendRequest()
            {
                CommentID = c.CommentID,
                AuthorID = c.AuthorID,
                PostID = c.PostID,
                Author = UserSendRequest.ToUserSendRequest(c.Author),
                Content = c.Content,
                PostTime = c.PostTime,
            };
        }
    }
}
