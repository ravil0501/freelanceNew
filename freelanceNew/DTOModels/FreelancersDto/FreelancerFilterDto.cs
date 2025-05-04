namespace freelanceNew.DTOModels.FreelancersDto
{
    public class FreelancerFilterDto
    {
        public decimal? MinHourlyRate { get; set; }
        public decimal? MaxHourlyRate { get; set; }
        public string Location { get; set; }
        public string SearchQuery { get; set; }
        public List<Guid> SkillIds { get; set; } = new();
    }
}
