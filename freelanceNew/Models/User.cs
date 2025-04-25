using freelanceNew.Models.Enums;

namespace freelanceNew.Models
{
    public class User
    {
        public Guid UserId { get; set; } // Primary Key
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
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
