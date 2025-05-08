using freelanceNew.Models.Enums;

namespace freelanceNew.DTOModels.ProposalsDto
{
    public class ProposalFilterDto
    {
        public Guid? JobId { get; set; }
        public Guid? FreelancerId { get; set; }
        public ProposalStatus? Status { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
