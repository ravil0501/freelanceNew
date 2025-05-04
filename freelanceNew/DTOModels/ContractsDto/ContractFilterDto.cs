namespace freelanceNew.DTOModels.ContractsDto
{
    // Для фильтрации контрактов
    public class ContractFilterDto
    {
        public string Status { get; set; }
        public Guid? FreelancerId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? JobId { get; set; }
    }
}
