using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        public string UniversityID { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}