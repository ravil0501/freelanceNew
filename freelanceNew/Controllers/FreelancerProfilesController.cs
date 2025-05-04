using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels.FreelancersDto;
using freelanceNew.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using freelanceNew.Models.Enums;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FreelancerProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FreelancerProfilesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/freelancerprofiles?minRate=50&maxRate=100&location=USA&skill=Programming
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FreelancerProfileDto>>> GetFreelancerProfiles(
            [FromQuery] FreelancerFilterDto filter)
        {
            var query = _context.FreelancerProfiles
                .Include(f => f.User)
                .Include(f => f.FreelancerSkills)
                .ThenInclude(fs => fs.Skill)
                .AsQueryable();

            // Apply filters
            if (filter.MinHourlyRate.HasValue)
                query = query.Where(f => f.HourlyRate >= filter.MinHourlyRate);

            if (filter.MaxHourlyRate.HasValue)
                query = query.Where(f => f.HourlyRate <= filter.MaxHourlyRate);

            if (!string.IsNullOrEmpty(filter.Location))
                query = query.Where(f => f.Location.Contains(filter.Location));

            if (!string.IsNullOrEmpty(filter.SearchQuery))
                query = query.Where(f =>
                    f.FullName.Contains(filter.SearchQuery) ||
                    f.Bio.Contains(filter.SearchQuery));

            if (filter.SkillIds != null && filter.SkillIds.Any())
                query = query.Where(f => f.FreelancerSkills.Any(s => filter.SkillIds.Contains(s.SkillId)));

            var profiles = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FreelancerProfileDto>>(profiles));
        }

        // GET: api/freelancerprofiles/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FreelancerProfileDetailsDto>> GetFreelancerProfile(Guid id)
        {
            var profile = await _context.FreelancerProfiles
                .Include(f => f.User)
                .Include(f => f.FreelancerSkills)
                .ThenInclude(fs => fs.Skill)
                .FirstOrDefaultAsync(f => f.FreelancerId == id);

            if (profile == null) return NotFound();

            return Ok(_mapper.Map<FreelancerProfileDetailsDto>(profile));
        }

        // POST: api/freelancerprofiles
        [HttpPost]
        [Authorize(Roles = "Freelancer,Admin")]
        public async Task<ActionResult<FreelancerProfileDetailsDto>> CreateFreelancerProfile(
            CreateFreelancerProfileDto dto)
        {
            var currentUserId = GetCurrentUserId();

            // Only allow creating profile for yourself unless admin
            if (dto.UserId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || user.Role != UserRole.Freelancer)
                return BadRequest("User must be a freelancer");

            var invalidSkills = await ValidateSkills(dto.SkillIds);
            if (invalidSkills.Any())
                return BadRequest($"Invalid skill IDs: {string.Join(", ", invalidSkills)}");

            var profile = _mapper.Map<FreelancerProfile>(dto);
            profile.FreelancerId = dto.UserId;

            profile.FreelancerSkills = dto.SkillIds.Select(skillId => new FreelancerSkill
            {
                FreelancerId = profile.FreelancerId,
                SkillId = skillId
            }).ToList();

            _context.FreelancerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetFreelancerProfile),
                new { id = profile.FreelancerId },
                _mapper.Map<FreelancerProfileDetailsDto>(profile));
        }

        // PUT: api/freelancerprofiles/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Freelancer,Admin")]
        public async Task<IActionResult> UpdateFreelancerProfile(
            Guid id,
            UpdateFreelancerProfileDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var profile = await _context.FreelancerProfiles
                .Include(f => f.FreelancerSkills)
                .FirstOrDefaultAsync(f => f.FreelancerId == id);

            if (profile == null) return NotFound();

            // Only allow updating your own profile unless admin
            if (profile.FreelancerId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            _mapper.Map(dto, profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/skills")]
        [Authorize(Roles = "Freelancer,Admin")]
        public async Task<IActionResult> UpdateFreelancerSkills(
            Guid id,
            UpdateSkillsDto dto)
        {
            var currentUserId = GetCurrentUserId();
            var profile = await _context.FreelancerProfiles
                .Include(f => f.FreelancerSkills)
                .FirstOrDefaultAsync(f => f.FreelancerId == id);

            if (profile == null) return NotFound();

            if (profile.FreelancerId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            var invalidSkills = await ValidateSkills(dto.SkillIds);
            if (invalidSkills.Any())
                return BadRequest($"Invalid skill IDs: {string.Join(", ", invalidSkills)}");

            profile.FreelancerSkills = dto.SkillIds.Select(skillId => new FreelancerSkill
            {
                FreelancerId = id,
                SkillId = skillId
            }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Freelancer,Admin")]
        public async Task<IActionResult> DeleteFreelancerProfile(Guid id)
        {
            var currentUserId = GetCurrentUserId();
            var profile = await _context.FreelancerProfiles.FindAsync(id);

            if (profile == null) return NotFound();

            if (profile.FreelancerId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            _context.FreelancerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<List<Guid>> ValidateSkills(List<Guid> skillIds)
        {
            if (skillIds == null || !skillIds.Any()) return new List<Guid>();

            var existingSkills = await _context.Skills
                .Where(s => skillIds.Contains(s.SkillId))
                .Select(s => s.SkillId)
                .ToListAsync();

            return skillIds.Except(existingSkills).ToList();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("sub") ??
                throw new UnauthorizedAccessException("User ID claim not found");
            return Guid.Parse(userIdClaim.Value);
        }
    }
}
