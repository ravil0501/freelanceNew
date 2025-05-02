using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.ClientsDto
{
    // DTO для обновления профиля клиента
    public class UpdateClientProfileDto
    {
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Url]
        [StringLength(200)]
        public string Website { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Location { get; set; }
    }
}
