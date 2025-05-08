namespace freelanceNew.DTOModels.ProposalsDto
{
    public class ProposalDto
    {
        public Guid ProposalId { get; set; }
        public Guid JobId { get; set; }
        public Guid FreelancerId { get; set; }
        public string CoverLetter { get; set; }
        public decimal ProposedRate { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Status { get; set; }
    }
}
