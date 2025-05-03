using freelanceNew.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.AuthenticationDto
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public UserRole Role { get; set; } // Enum: Admin, Freelancer, Client
    }
}
