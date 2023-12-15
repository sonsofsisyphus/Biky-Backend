using System.ComponentModel.DataAnnotations;
using Entities;

namespace Services.DTO
{
    public class UserSendRequest
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string Nickname { get; set; }

        public string? ProfileImage { get; set; }

        public static UserSendRequest ToUserSendRequest(User u)
        {
            return new UserSendRequest { UserID = u.UserID, Nickname = u.Nickname, ProfileImage = u.ProfileImage };
        }

    }
}
