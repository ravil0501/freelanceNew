namespace freelanceNew.DTOModels.FreelancersDto
{
    public class FreelancerReviewDto
    {
        public Guid ReviewId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
