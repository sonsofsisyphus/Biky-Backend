using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class UserLoginRequest
    {
        [Required]
        public string Nickname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
