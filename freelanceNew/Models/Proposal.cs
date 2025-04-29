using freelanceNew.Models.Enums;
using System.ComponentModel.DataAnnotations;


namespace freelanceNew.Models
{
    public class Proposal
    {
        [Key]
        public Guid ProposalId { get; set; } // Primary Key
        [Required]
        public Guid JobId { get; set; } // Foreign Key to Job
        [Required]
        public Guid FreelancerId { get; set; } // Foreign Key to FreelancerProfile
        public string CoverLetter { get; set; }
        public decimal ProposedRate { get; set; }
        public DateTime SubmittedAt { get; set; }
        [Required]
        public ProposalStatus Status { get; set; } // Enum: Pending, Accepted, Rejected

        // Navigation Properties
        public Job Job { get; set; }
        public FreelancerProfile FreelancerProfile { get; set; }
    }

}
