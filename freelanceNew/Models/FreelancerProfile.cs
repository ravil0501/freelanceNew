using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace freelanceNew.Models
{
    public class FreelancerProfile
    {
        [Key]
        public Guid FreelancerId { get; set; } // Primary Key and Foreign Key to User
        [Required, MaxLength(200)]
        public string FullName { get; set; }
        [MaxLength(1000)]
        public string Bio { get; set; }
        public string PortfolioUrl { get; set; }
        public decimal HourlyRate { get; set; }
        public string Location { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<FreelancerSkill> FreelancerSkills { get; set; }
        public ICollection<Proposal> Proposals { get; set; }
        public ICollection<Contract> Contracts { get; set; }
    }


}
