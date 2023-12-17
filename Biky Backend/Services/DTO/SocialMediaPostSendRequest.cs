using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class SocialMediaPostSendRequest : PostSendRequest
    {
        [Required]
        public int Likes { get; set; }

        public bool IsLiked { get; set; } = false;

        [Required]
        public bool IsAnonymous { get; set; }

        public static SocialMediaPostSendRequest ToSocialMediaPostSendRequest(SocialMediaPost p, int like, bool isLiked)
        {

            SocialMediaPostSendRequest s = new SocialMediaPostSendRequest (ToPostSendRequest(p), p.IsAnonymous);
            s.Likes = like;
            s.IsLiked = isLiked;
            s.IsAnonymous = p.IsAnonymous;
            return s;
        }

        public SocialMediaPostSendRequest(PostSendRequest p, bool isAnonymous)
        {
            PostID = p.PostID;
            AuthorID = p.AuthorID;
            ContentText = p.ContentText;
            Images = p.Images;
            if(!isAnonymous) Author = p.Author;
            else
            {
                Author = new UserSendRequest()
                {
                    Nickname = "anonymous",
                    ProfileImage = "",
                    UserID = Guid.Empty
                };
            }
            PostTime = p.PostTime;
        }
    }
}
