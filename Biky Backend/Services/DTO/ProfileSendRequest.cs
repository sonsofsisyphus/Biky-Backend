using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Entities;

namespace Services.DTO
{
    public class ProfileSendRequest : UserSendRequest
    {
        [Required]
        public int FollowersNumber { get; set; }

        [Required]
        public int FollowingsNumber { get; set; }

        [Required]
        public int PostNumber { get; set; }

        [Required]
        public int LikeNumber { get; set; }

        public string Description { get; set; }

        public ProfileSendRequest(UserSendRequest u, int follows, int followings, int posts, int likes, string description)
        {
            UserID = u.UserID;
            Nickname = u.Nickname;
            ProfileImage = u.ProfileImage;
            FollowersNumber = follows;
                FollowingsNumber = followings;
                PostNumber = posts;
            LikeNumber = likes;
            Description = description;
        }
    }
}
