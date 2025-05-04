namespace freelanceNew.DTOModels.FreelancersDto
{
    public class FreelancerProfileDetailsDto : FreelancerProfileDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<FreelancerReviewDto> Reviews { get; set; } = new();
        public List<FreelancerContractDto> Contracts { get; set; } = new();
    }
}
