namespace freelanceNew.DTOModels.JobsDto
{
    public class JobsFilterDto
    {
        public string? Status { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? SearchQuery { get; set; }
    }
}
