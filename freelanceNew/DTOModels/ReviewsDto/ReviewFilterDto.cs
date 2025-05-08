using System.ComponentModel.DataAnnotations;

namespace freelanceNew.DTOModels.ReviewsDto
{
    public class ReviewFilterDto
    {
        public Guid? ContractId { get; set; }
        public Guid? ReviewerId { get; set; }
        public Guid? RevieweeId { get; set; }

        [Range(1, 5)]
        public int? MinRating { get; set; }

        [Range(1, 5)]
        public int? MaxRating { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [Range(1, 1000)]
        public int PageSize { get; set; } = 20;

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
    }
}
