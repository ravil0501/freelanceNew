using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Security.Claims;
using freelanceNew.DTOModels.SkillsDto;
using freelanceNew.Models;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SkillsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/skills?searchQuery=design
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills([FromQuery] SkillsFilterDto filter)
        {
            var query = _context.Skills.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                var q = filter.SearchQuery.ToLower();
                query = query.Where(s => s.Name.ToLower().Contains(q));
            }

            var skills = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SkillDto>>(skills));
        }

        // GET: api/skills/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SkillDto>> GetSkill(Guid id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            return Ok(_mapper.Map<SkillDto>(skill));
        }

        // POST: api/skills
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SkillDto>> CreateSkill(CreateSkillDto dto)
        {
            var skill = _mapper.Map<Skill>(dto);
            skill.SkillId = Guid.NewGuid();

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSkill), new { id = skill.SkillId }, _mapper.Map<SkillDto>(skill));
        }

        // PUT: api/skills/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSkill(Guid id, UpdateSkillDto dto)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            skill.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/skills/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null) return NotFound();

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

