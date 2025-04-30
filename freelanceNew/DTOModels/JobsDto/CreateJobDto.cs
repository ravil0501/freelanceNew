using freelanceNew.Models.Enums;

namespace freelanceNew.DTOModels.JobsDto
{
    public class CreateJobDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = JobStatus.Open.ToString(); // По умолчанию "Open"
    }
}
