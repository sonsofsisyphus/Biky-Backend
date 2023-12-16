using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class UserLoginSendRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public Guid UserID { get; set; }
    }
}
