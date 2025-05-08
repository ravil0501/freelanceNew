namespace freelanceNew.DTOModels.ProposalsDto
{
    public class CreateProposalDto
    {
        public Guid JobId { get; set; }
        public Guid FreelancerId { get; set; }
        public string CoverLetter { get; set; }
        public decimal ProposedRate { get; set; }
    }
}
