using freelanceNew.Models.Enums;


namespace freelanceNew.Models
{
    public class Proposal
    {
        public Guid ProposalId { get; set; } // Primary Key
        public Guid JobId { get; set; } // Foreign Key to Job
        public Guid FreelancerId { get; set; } // Foreign Key to FreelancerProfile
        public string CoverLetter { get; set; }
        public decimal ProposedRate { get; set; }
        public DateTime SubmittedAt { get; set; }
        public ProposalStatus Status { get; set; } // Enum: Pending, Accepted, Rejected

        // Navigation Properties
        public Job Job { get; set; }
        public FreelancerProfile FreelancerProfile { get; set; }
    }

}
