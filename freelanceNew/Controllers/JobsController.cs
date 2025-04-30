using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels;
using freelanceNew.Models;
using freelanceNew.Models.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using freelanceNew.DTOModels.JobsDto;

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

        // GET: api/jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<JobDto>>(jobs));
        }

        // GET: api/jobs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            return Ok(_mapper.Map<JobDto>(job));
        }

        // POST: api/jobs
        [HttpPost]
        public async Task<ActionResult<JobDto>> CreateJob(CreateJobDto dto)
        {
            if (!Enum.TryParse<JobStatus>(dto.Status, out var status))
                return BadRequest("Invalid job status.");

            var job = new Job
            {
                JobId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(), // Здесь нужно получить ClientId из аутентификации
                Title = dto.Title,
                Description = dto.Description,
                Budget = dto.Budget,
                PostedAt = DateTime.UtcNow,
                Deadline = dto.Deadline,
                Status = status
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<JobDto>(job);
            return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, resultDto);
        }

        // PUT: api/jobs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, UpdateJobDto dto)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

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
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
