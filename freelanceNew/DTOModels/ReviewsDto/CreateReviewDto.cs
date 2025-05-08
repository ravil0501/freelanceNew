using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.ReviewsDto
{
    public class CreateReviewDto
    {
        [Required]
        public Guid ContractId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 1000 characters.")]
        public string Comment { get; set; }
    }
}
