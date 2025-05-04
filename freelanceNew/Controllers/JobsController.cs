using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AutoMapper;
using freelanceNew.DTOModels.JobsDto;
using freelanceNew.Models;
using freelanceNew.Models.Enums;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/jobs?status=Open&minBudget=100&searchQuery=design
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs([FromQuery] JobsFilterDto filter)
        {
            var query = _context.Jobs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                Enum.TryParse<JobStatus>(filter.Status, out var status))
                query = query.Where(j => j.Status == status);

            if (filter.MinBudget.HasValue)
                query = query.Where(j => j.Budget >= filter.MinBudget.Value);

            if (filter.MaxBudget.HasValue)
                query = query.Where(j => j.Budget <= filter.MaxBudget.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                var q = filter.SearchQuery.ToLower();
                query = query.Where(j => j.Title.ToLower().Contains(q) || j.Description.ToLower().Contains(q));
            }

            var jobs = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<JobDto>>(jobs));
        }

        // GET: api/jobs/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<JobDto>> GetJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            return Ok(_mapper.Map<JobDto>(job));
        }

        // POST: api/jobs
        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        public async Task<ActionResult<JobDto>> CreateJob(CreateJobDto dto)
        {
            if (!Enum.TryParse<JobStatus>(dto.Status, out var status))
                return BadRequest("Invalid job status.");

            var currentUserId = GetCurrentUserId();

            var job = new Job
            {
                JobId = Guid.NewGuid(),
                ClientId = currentUserId,
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                PostedAt = DateTime.UtcNow,
                Deadline = dto.Deadline,
                Status = status
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, _mapper.Map<JobDto>(job));
        }

        // PUT: api/jobs/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> UpdateJob(Guid id, UpdateJobDto dto)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            if (job.ClientId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            if (!Enum.TryParse<JobStatus>(dto.Status, out var status))
                return BadRequest("Invalid job status.");

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Budget = dto.Budget;
            job.Deadline = dto.Deadline;
            job.Status = status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/jobs/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            if (job.ClientId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found.");
            return Guid.Parse(userIdClaim.Value);
        }
    }
}

