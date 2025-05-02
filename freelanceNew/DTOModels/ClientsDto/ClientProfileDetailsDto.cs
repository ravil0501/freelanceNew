namespace freelanceNew.DTOModels.ClientsDto
{
    // Расширенный DTO для деталей клиента
    public class ClientProfileDetailsDto : ClientProfileDto
    {
        public int ActiveJobsCount { get; set; }
        public int CompletedContractsCount { get; set; }
        public DateTime MemberSince { get; set; }
    }
}
