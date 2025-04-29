using freelanceNew.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace freelanceNew.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } // Primary Key

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; } // Enum: Admin, Freelancer, Client
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public FreelancerProfile FreelancerProfile { get; set; }
        public ClientProfile ClientProfile { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<Review> ReviewsGiven { get; set; }
        public ICollection<Review> ReviewsReceived { get; set; }
    }
}
