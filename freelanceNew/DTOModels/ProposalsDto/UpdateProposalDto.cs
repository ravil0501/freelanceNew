using freelanceNew.Models.Enums;

namespace freelanceNew.DTOModels.ProposalsDto
{
    public class UpdateProposalDto
    {
        public string CoverLetter { get; set; }
        public decimal ProposedRate { get; set; }
        public ProposalStatus Status { get; set; }
    }
}
