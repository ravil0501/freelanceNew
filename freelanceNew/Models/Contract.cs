using freelanceNew.Models.Enums;
using System.ComponentModel.DataAnnotations;


namespace freelanceNew.Models
{
    public class Contract
    {
        [Key]
        public Guid ContractId { get; set; } // Primary Key
        [Required]
        public Guid JobId { get; set; } // Foreign Key to Job
        [Required]
        public Guid FreelancerId { get; set; } // Foreign Key to FreelancerProfile
        [Required]
        public Guid ClientId { get; set; } // Foreign Key to ClientProfile
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal AgreedRate { get; set; }
        public ContractStatus Status { get; set; } // Enum: Active, Completed, Terminated

        // Navigation Properties
        public Job Job { get; set; }
        public FreelancerProfile FreelancerProfile { get; set; }
        public ClientProfile ClientProfile { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }


}
