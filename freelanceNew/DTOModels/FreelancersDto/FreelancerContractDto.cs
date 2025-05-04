using freelanceNew.Models.Enums;

namespace freelanceNew.DTOModels.FreelancersDto
{
    public class FreelancerContractDto
    {
        public Guid ContractId { get; set; }
        public string JobTitle { get; set; }
        public ContractStatus Status { get; set; }
    }
}
