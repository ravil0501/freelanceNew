using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

using freelanceNew.Models.Enums;


namespace freelanceNew.Models
{
    public class Job
    {
        [Key]
        public Guid JobId { get; set; } // Primary Key
        [Required]
        public Guid ClientId { get; set; } // Foreign Key to ClientProfile
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime Deadline { get; set; }
        [Required]
        public JobStatus Status { get; set; } // Enum: Open, InProgress, Completed, Cancelled

        // Navigation Properties
        public ClientProfile ClientProfile { get; set; }
        public ICollection<Proposal> Proposals { get; set; }
        public Contract Contract { get; set; }
    }
}
