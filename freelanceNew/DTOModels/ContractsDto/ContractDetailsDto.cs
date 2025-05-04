using freelanceNew.DTOModels.ClientsDto;
using freelanceNew.DTOModels.FreelancersDto;
using freelanceNew.DTOModels.JobsDto;
using freelanceNew.DTOModels.UsersDTO;

namespace freelanceNew.DTOModels.ContractsDto
{
    public class ContractDetailsDto
    {
        public Guid ContractId { get; set; }
        public Guid JobId { get; set; }
        public Guid FreelancerId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal AgreedRate { get; set; }
        public string Status { get; set; }

        // Детали связанных сущностей
        public JobDto Job { get; set; }
        public FreelancerProfileDto FreelancerProfile { get; set; }
        public ClientProfileDto ClientProfile { get; set; }
    }
}
