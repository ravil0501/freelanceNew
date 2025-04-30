using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.ContractsDto
{
    public class CreateContractDto
    {
        [Required]
        public Guid JobId { get; set; }

        [Required]
        public Guid FreelancerId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public decimal AgreedRate { get; set; }

        [Required]
        public string Status { get; set; } // Enum as string
    }
}
