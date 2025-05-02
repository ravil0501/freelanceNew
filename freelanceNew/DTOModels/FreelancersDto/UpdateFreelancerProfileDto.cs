using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.FreelancersDto
{
    public class UpdateFreelancerProfileDto
    {
        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(1000)]
        public string Bio { get; set; }

        [Url]
        public string PortfolioUrl { get; set; }

        [Range(0, 1000)]
        public decimal? HourlyRate { get; set; }

        public string Location { get; set; }

        // Updated skills list
        public List<Guid> SkillIds { get; set; } = new();
    }
}
