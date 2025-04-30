namespace freelanceNew.DTOModels.JobsDto
{
    public class UpdateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
    }
}
