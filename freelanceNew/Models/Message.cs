namespace freelanceNew.Models
{
    public class Message
    {
        public Guid MessageId { get; set; } // Primary Key
        public Guid SenderId { get; set; } // Foreign Key to User
        public Guid ReceiverId { get; set; } // Foreign Key to User
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

        // Navigation Properties
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }


}
