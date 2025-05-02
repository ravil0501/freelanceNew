using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels;
using freelanceNew.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using freelanceNew.DTOModels.ClientsDto;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ClientProfilesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/clientprofiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientProfileDto>>> GetClientProfiles()
        {
            var profiles = await _context.ClientProfiles
                .Include(c => c.User) // Включаем связанного пользователя
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ClientProfileDto>>(profiles));
        }

        // GET: api/clientprofiles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientProfileDto>> GetClientProfile(Guid id)
        {
            var profile = await _context.ClientProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (profile == null) return NotFound();

            return Ok(_mapper.Map<ClientProfileDto>(profile));
        }

        // POST: api/clientprofiles
        [HttpPost]
        public async Task<ActionResult<ClientProfileDto>> CreateClientProfile(CreateClientProfileDto dto)
        {
            // Проверяем, существует ли пользователь
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var profile = new ClientProfile
            {
                ClientId = dto.UserId, // Используем UserId как ClientId
                CompanyName = dto.CompanyName,
                Website = dto.Website,
                Description = dto.Description,
                Location = dto.Location
            };

            _context.ClientProfiles.Add(profile);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<ClientProfileDto>(profile);
            return CreatedAtAction(nameof(GetClientProfile), new { id = profile.ClientId }, resultDto);
        }

        // PUT: api/clientprofiles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientProfile(Guid id, UpdateClientProfileDto dto)
        {
            var profile = await _context.ClientProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _mapper.Map(dto, profile); // Автоматическое обновление полей
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/clientprofiles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientProfile(Guid id)
        {
            var profile = await _context.ClientProfiles.FindAsync(id);
            if (profile == null) return NotFound();

            _context.ClientProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
