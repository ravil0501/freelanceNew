using System.ComponentModel.DataAnnotations; // Для динамического LINQ

namespace freelanceNew.DTOModels.ClientsDto
{
    // DTO для фильтрации
    public class ClientProfileFilterDto
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public string CompanyName { get; set; }
        public string Location { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public string SortBy { get; set; } = "CompanyName";
        public bool SortDescending { get; set; } = false;
    }
}
