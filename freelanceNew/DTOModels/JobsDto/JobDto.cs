namespace freelanceNew.DTOModels.JobsDto
{
    public class JobDto
    {
        public Guid JobId { get; set; }
        public Guid ClientId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; } // Используем string для сериализации enum
    }
}
