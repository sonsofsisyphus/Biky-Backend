using System.ComponentModel.DataAnnotations;

namespace Services.DTO
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "University ID is required.")]
        public string UniversityID { get; set; }

        [Required(ErrorMessage = "Nickname is required.")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}