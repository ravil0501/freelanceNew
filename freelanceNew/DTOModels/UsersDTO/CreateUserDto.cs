using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.UsersDTO
{
    public class CreateUserDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [RegularExpression("Admin|Freelancer|Client", ErrorMessage = "Role must be either Admin, Freelancer, or Client.")]
        public string Role { get; set; }
    }
}
