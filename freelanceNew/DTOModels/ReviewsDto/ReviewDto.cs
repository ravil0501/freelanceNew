using System;
using freelanceNew.Models.Enums;

namespace freelanceNew.DTOModels.ReviewsDto
{
    public class ReviewDto
    {
        public Guid ReviewId { get; set; }
        public Guid ContractId { get; set; }

        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; }

        public Guid RevieweeId { get; set; }
        public string RevieweeName { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewedAt { get; set; }
    }
}
