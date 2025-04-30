using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.UsersDTO
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [RegularExpression("Admin|Freelancer|Client", ErrorMessage = "Role must be either Admin, Freelancer, or Client.")]
        public string Role { get; set; }
    }
}
