using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels.FreelancersDto;
using freelanceNew.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreelancerProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FreelancerProfilesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/freelancerprofiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreelancerProfileDto>>> GetFreelancerProfiles()
        {
            var profiles = await _context.FreelancerProfiles
                .Include(f => f.User)
                .Include(f => f.FreelancerSkills)
                .ThenInclude(fs => fs.Skill)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<FreelancerProfileDto>>(profiles));
        }

        // GET: api/freelancerprofiles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FreelancerProfileDto>> GetFreelancerProfile(Guid id)
        {
            var profile = await _context.FreelancerProfiles
                .Include(f => f.User)
                .Include(f => f.FreelancerSkills)
                .ThenInclude(fs => fs.Skill)
                .FirstOrDefaultAsync(f => f.FreelancerId == id);

            if (profile == null) return NotFound();

            return Ok(_mapper.Map<FreelancerProfileDto>(profile));
        }


        [HttpPost]
        public async Task<ActionResult<FreelancerProfileDto>> CreateFreelancerProfile(
            CreateFreelancerProfileDto dto)
        {
            // Validate user exists
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest("User not found");

            // Validate skills exist
            var invalidSkills = dto.SkillIds.Except(
                await _context.Skills.Where(s => dto.SkillIds.Contains(s.SkillId))
                                    .Select(s => s.SkillId)
                                    .ToListAsync());

            if (invalidSkills.Any())
            {
                return BadRequest($"Invalid skill IDs: {string.Join(", ", invalidSkills)}");
            }

            var profile = _mapper.Map<FreelancerProfile>(dto);
            profile.FreelancerId = dto.UserId;

            // Add skills
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
                await GetFullProfileDto(profile.FreelancerId));
        }

        [HttpPut("{id}/skills")]
        public async Task<IActionResult> UpdateFreelancerSkills(
            Guid id,
            List<Guid> skillIds)
        {
            var profile = await _context.FreelancerProfiles
                .Include(f => f.FreelancerSkills)
                .FirstOrDefaultAsync(f => f.FreelancerId == id);

            if (profile == null) return NotFound();

            // Validate skills
            var existingSkills = await _context.Skills
                .Where(s => skillIds.Contains(s.SkillId))
                .Select(s => s.SkillId)
                .ToListAsync();

            var invalidSkills = skillIds.Except(existingSkills);
            if (invalidSkills.Any())
            {
                return BadRequest($"Invalid skill IDs: {string.Join(", ", invalidSkills)}");
            }

            // Update skills
            profile.FreelancerSkills = existingSkills
                .Select(skillId => new FreelancerSkill
                {
                    FreelancerId = id,
                    SkillId = skillId
                }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<FreelancerProfileDto> GetFullProfileDto(Guid freelancerId)
        {
            var profile = await _context.FreelancerProfiles
                .Include(f => f.User)
                .Include(f => f.FreelancerSkills)
                .ThenInclude(fs => fs.Skill)
                .FirstOrDefaultAsync(f => f.FreelancerId == freelancerId);

            return _mapper.Map<FreelancerProfileDto>(profile);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreelancerProfile(Guid id)
        {
            var profile = await _context.FreelancerProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _context.FreelancerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
