using Entities;
using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class SocialMediaPostAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public string ContentText { get; set; }

        public List<String>? Images { get; set; }

        [Required]
        public bool IsAnonymous { get; set; }

        public SocialMediaPost ToSocialMediaPost()
        {
            return new SocialMediaPost
            {
                PostID = Guid.NewGuid(),
                AuthorID = AuthorID,
                ContentText = ContentText,
                PostTime = DateTime.Now,
                IsAnonymous = IsAnonymous
            };
        }
    }
}
