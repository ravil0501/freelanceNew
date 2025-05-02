using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.FreelancersDto
{
    // For creating new freelancer profile (POST requests)
    public class CreateFreelancerProfileDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        [Url]
        public string PortfolioUrl { get; set; }

        [Range(0, 1000)]
        public decimal HourlyRate { get; set; }

        public string Location { get; set; }

        // List of Skill GUIDs
        public List<Guid> SkillIds { get; set; } = new();
    }
}
