namespace freelanceNew.Models
{
    public class Review
    {
        public Guid ReviewId { get; set; } // Primary Key
        public Guid ContractId { get; set; } // Foreign Key to Contract
        public Guid ReviewerId { get; set; } // Foreign Key to User
        public Guid RevieweeId { get; set; } // Foreign Key to User
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; }
        public DateTime ReviewedAt { get; set; }

        // Navigation Properties
        public Contract Contract { get; set; }
        public User Reviewer { get; set; }
        public User Reviewee { get; set; }
    }


}
