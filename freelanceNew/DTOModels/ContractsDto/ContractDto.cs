namespace freelanceNew.DTOModels.ContractsDto
{
    public class ContractDto
    {
        public Guid ContractId { get; set; }
        public Guid JobId { get; set; }
        public Guid FreelancerId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal AgreedRate { get; set; }
        public string Status { get; set; } // enum as string
    }
}
