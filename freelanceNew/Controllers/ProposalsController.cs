using AutoMapper;
using freelanceNew.DTOModels.ProposalsDto;
using freelanceNew.Models;
using freelanceNew.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace freelanceNew.Controllers
{
    [Authorize(Roles = "Admin,Freelancer,Client")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProposalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProposalsController> _logger;

        public ProposalsController(ApplicationDbContext context, IMapper mapper, ILogger<ProposalsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить список предложений с фильтрацией, сортировкой и пагинацией
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProposalPagedResult<ProposalDto>>> GetProposals([FromQuery] ProposalFilterDto filter)
        {
            try
            {
                var query = _context.Proposals
                    .Include(p => p.Job)
                    .Include(p => p.FreelancerProfile)
                    .AsQueryable();

                if (filter.JobId.HasValue)
                    query = query.Where(p => p.JobId == filter.JobId);

                if (filter.FreelancerId.HasValue)
                    query = query.Where(p => p.FreelancerId == filter.FreelancerId);

                if (filter.Status.HasValue)
                    query = query.Where(p => p.Status == filter.Status);

                if (!string.IsNullOrEmpty(filter.SortBy))
                {
                    try
                    {
                        query = query.OrderBy($"{filter.SortBy} {(filter.SortDescending ? "desc" : "asc")}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Invalid sort parameter");
                        return BadRequest("Invalid sort parameter");
                    }
                }
                else
                {
                    query = query.OrderByDescending(p => p.SubmittedAt);
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return Ok(new ProposalPagedResult<ProposalDto>
                {
                    Items = _mapper.Map<List<ProposalDto>>(items),
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching proposals");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить предложение по ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProposalDto>> GetProposal(Guid id)
        {
            var proposal = await _context.Proposals
                .Include(p => p.Job)
                .Include(p => p.FreelancerProfile)
                .FirstOrDefaultAsync(p => p.ProposalId == id);

            if (proposal == null)
                return NotFound();

            return Ok(_mapper.Map<ProposalDto>(proposal));
        }

        /// <summary>
        /// Создать новое предложение
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProposalDto>> CreateProposal([FromBody] CreateProposalDto dto)
        {
            if (!await _context.FreelancerProfiles.AnyAsync(f => f.FreelancerId == dto.FreelancerId))
                return BadRequest("Freelancer not found");

            if (!await _context.Jobs.AnyAsync(j => j.JobId == dto.JobId))
                return BadRequest("Job not found");

            var proposal = _mapper.Map<Proposal>(dto);
            proposal.ProposalId = Guid.NewGuid();
            proposal.SubmittedAt = DateTime.UtcNow;
            proposal.Status = ProposalStatus.Pending;

            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ProposalDto>(proposal);
            return CreatedAtAction(nameof(GetProposal), new { id = result.ProposalId }, result);
        }

        /// <summary>
        /// Обновить существующее предложение
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProposal(Guid id, [FromBody] UpdateProposalDto dto)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
                return NotFound();

            _mapper.Map(dto, proposal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить предложение
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProposal(Guid id)
        {
            var proposal = await _context.Proposals.FindAsync(id);
            if (proposal == null)
                return NotFound();

            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

