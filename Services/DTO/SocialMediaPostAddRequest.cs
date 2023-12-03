using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class SocialMediaPostAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public string ContentText { get; set; }

        public ImageCollection Images { get; set; }

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
                Images = Images,
                LikeCount = 0,
                IsAnonymous = IsAnonymous

            };
        }

    }
}
