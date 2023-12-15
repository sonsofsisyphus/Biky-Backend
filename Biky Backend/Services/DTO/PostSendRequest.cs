using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class PostSendRequest
    {
        [Required]
        public Guid PostID { get; set; }

        public Guid AuthorID { get; set; }

        [Required]
        public string ContentText { get; set; }

        public List<String>? Images { get; set; }

        //[Required]
        public UserSendRequest? Author { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        public static PostSendRequest ToPostSendRequest(Post p)
        {
            //Console.Write(p.Author.Nickname);
            return new PostSendRequest()
            {
                PostID = p.PostID,
                AuthorID = p.AuthorID,
                ContentText = p.ContentText,
                Author = UserSendRequest.ToUserSendRequest(p.Author),
                PostTime = p.PostTime
            };
        }

    }
}