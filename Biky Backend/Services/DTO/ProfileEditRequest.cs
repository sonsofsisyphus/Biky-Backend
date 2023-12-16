using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ProfileEditRequest
    {
        public Guid userID { get; set; }

        public string? Nickname { get; set; }

        public string? ProfileImage { get; set; }

        public string? Description { get; set; }
    }
}
