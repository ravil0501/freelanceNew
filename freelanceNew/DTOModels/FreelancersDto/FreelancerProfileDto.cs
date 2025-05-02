namespace freelanceNew.DTOModels.FreelancersDto
{
    public class FreelancerProfileDto
    {
        public Guid FreelancerId { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string PortfolioUrl { get; set; }
        public decimal HourlyRate { get; set; }
        public string Location { get; set; }

        // User info
        public string UserName { get; set; }
        public string Email { get; set; }

        // Skills with GUIDs
        public List<FreelancerSkillDto> Skills { get; set; } = new();
    }
}
