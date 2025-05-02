namespace freelanceNew.DTOModels.ClientsDto
{
    public class ClientProfileDto
    {
        public Guid ClientId { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        // Опционально: можно включить базовую информацию о пользователе
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
