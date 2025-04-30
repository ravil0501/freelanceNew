using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.ContractsDto
{
    public class UpdateContractDto
    {
        [Required]
        public decimal AgreedRate { get; set; }

        [Required]
        public string Status { get; set; } // Enum as string

        public DateTime? EndDate { get; set; }
    }
}
