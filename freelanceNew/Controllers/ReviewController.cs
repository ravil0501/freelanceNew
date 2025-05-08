using AutoMapper;
using freelanceNew.DTOModels;
using freelanceNew.DTOModels.ReviewsDto;
using freelanceNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/reviews
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews([FromQuery] ReviewFilterDto filter)
        {
            var query = _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .AsQueryable();

            if (filter.ContractId.HasValue)
                query = query.Where(r => r.ContractId == filter.ContractId);

            if (filter.ReviewerId.HasValue)
                query = query.Where(r => r.ReviewerId == filter.ReviewerId);

            if (filter.RevieweeId.HasValue)
                query = query.Where(r => r.RevieweeId == filter.RevieweeId);

            if (filter.MinRating.HasValue)
                query = query.Where(r => r.Rating >= filter.MinRating);

            if (filter.MaxRating.HasValue)
                query = query.Where(r => r.Rating <= filter.MaxRating);

            if (filter.FromDate.HasValue)
                query = query.Where(r => r.ReviewedAt >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(r => r.ReviewedAt <= filter.ToDate);

            var reviews = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result = reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ContractId = r.ContractId,
                ReviewerId = r.ReviewerId,
                ReviewerName = r.Reviewer?.Username,
                RevieweeId = r.RevieweeId,
                RevieweeName = r.Reviewee?.Username,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewedAt = r.ReviewedAt
            });

            return Ok(result);
        }

        // POST: api/reviews
        [HttpPost]
        [Authorize(Roles = "Freelancer,Client")]
        public async Task<ActionResult<ReviewDto>> CreateReview([FromBody] CreateReviewDto dto)
        {
            var currentUserId = GetCurrentUserId();

            var contract = await _context.Contracts
                .Include(c => c.FreelancerId)
                .Include(c => c.ClientId)
                .FirstOrDefaultAsync(c => c.ContractId == dto.ContractId);

            if (contract == null)
                return NotFound("Contract not found.");

            // Проверяем, участвовал ли пользователь в контракте
            if (contract.FreelancerId != currentUserId && contract.ClientId != currentUserId)
                return Forbid();

            var existingReview = await _context.Reviews.FirstOrDefaultAsync(r =>
                r.ContractId == dto.ContractId && r.ReviewerId == currentUserId);

            if (existingReview != null)
                return BadRequest("Review already exists for this contract by the current user.");

            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                ContractId = dto.ContractId,
                ReviewerId = currentUserId,
                RevieweeId = contract.FreelancerId == currentUserId ? contract.ClientId : contract.FreelancerId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var result = new ReviewDto
            {
                ReviewId = review.ReviewId,
                ContractId = review.ContractId,
                ReviewerId = review.ReviewerId,
                ReviewerName = (await _context.Users.FindAsync(review.ReviewerId))?.Username,
                RevieweeId = review.RevieweeId,
                RevieweeName = (await _context.Users.FindAsync(review.RevieweeId))?.Username,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewedAt = review.ReviewedAt
            };

            return CreatedAtAction(nameof(GetReviews), new { id = review.ReviewId }, result);
        }

        // PUT: api/reviews/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Freelancer,Client")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] UpdateReviewDto dto)
        {
            var currentUserId = GetCurrentUserId();

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null) return NotFound();
            if (review.ReviewerId != currentUserId) return Forbid();

            if (dto.Rating.HasValue)
                review.Rating = dto.Rating.Value;

            if (!string.IsNullOrEmpty(dto.Comment))
                review.Comment = dto.Comment;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Freelancer,Client,Admin")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var review = await _context.Reviews.FindAsync(id);

            if (review == null) return NotFound();

            if (review.ReviewerId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("User ID claim not found");

            return Guid.Parse(userIdClaim.Value);
        }
    }
}
