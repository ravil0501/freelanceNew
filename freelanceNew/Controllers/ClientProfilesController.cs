using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels;
using freelanceNew.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using freelanceNew.DTOModels.ClientsDto;
using System.Linq.Dynamic.Core;
using freelanceNew.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace freelanceNew.Controllers
{
    /// <summary>
    /// Контроллер для работы с профилями клиентов
    /// </summary>
    [Authorize(Roles = "Admin,Client")]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientProfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientProfilesController> _logger;

        public ClientProfilesController(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<ClientProfilesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получить список профилей с пагинацией и фильтрацией
        /// </summary>
        /// <remarks>
        /// Примеры запросов:
        /// GET /api/clientprofiles
        /// GET /api/clientprofiles?page=2&amp;pageSize=10
        /// GET /api/clientprofiles?companyName=Tech&amp;location=NY
        /// GET /api/clientprofiles?sortBy=CompanyName&amp;sortDescending=true
        /// GET /api/clientprofiles?sortBy=User.Email&amp;sortDescending=false
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DTOModels.ClientsDto.PagedResult<ClientProfileDto>>> GetClientProfiles(
            [FromQuery] ClientProfileFilterDto filter)
        {
            try
            {
                var query = _context.ClientProfiles
                    .Include(c => c.User)
                    .AsQueryable();

                // Фильтрация
                if (!string.IsNullOrEmpty(filter.CompanyName))
                    query = query.Where("CompanyName.Contains(@0)", filter.CompanyName);

                if (!string.IsNullOrEmpty(filter.Location))
                    query = query.Where("Location.Contains(@0)", filter.Location);

                // Сортировка
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
                    query = query.OrderBy(c => c.CompanyName);
                }

                // Пагинация
                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();

                return Ok(new DTOModels.ClientsDto.PagedResult<ClientProfileDto>
                {
                    Items = _mapper.Map<List<ClientProfileDto>>(items),
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client profiles");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить профиль клиента по ID
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// GET /api/clientprofiles/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientProfileDetailsDto>> GetClientProfile(Guid id)
        {
            var profile = await _context.ClientProfiles
                .Include(c => c.User)
                .Include(c => c.Jobs)
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (profile == null)
                return NotFound();

            var dto = _mapper.Map<ClientProfileDetailsDto>(profile);
            dto.ActiveJobsCount = profile.Jobs.Count(j => j.Status == JobStatus.Open);
            dto.CompletedContractsCount = profile.Contracts.Count(c => c.Status == ContractStatus.Completed);

            return Ok(dto);
        }

        /// <summary>
        /// Создать новый профиль клиента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// POST /api/clientprofiles
        /// {
        ///     "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "companyName": "Tech Corp",
        ///     "website": "https://tech.com",
        ///     "description": "Технологическая компания",
        ///     "location": "New York"
        /// }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientProfileDto>> CreateClientProfile(
            [FromBody] CreateClientProfileDto dto)
        {
            if (!await _context.Users.AnyAsync(u => u.UserId == dto.UserId))
                return BadRequest("User not found");

            var profile = _mapper.Map<ClientProfile>(dto);
            profile.ClientId = dto.UserId;

            _context.ClientProfiles.Add(profile);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<ClientProfileDto>(profile);
            return CreatedAtAction(nameof(GetClientProfile),
                new { id = profile.ClientId },
                resultDto);
        }

        /// <summary>
        /// Обновить профиль клиента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// PUT /api/clientprofiles/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// {
        ///     "companyName": "New Tech Corp",
        ///     "website": "https://newtech.com",
        ///     "description": "Обновлённое описание",
        ///     "location": "Boston"
        /// }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClientProfile(
            Guid id,
            [FromBody] UpdateClientProfileDto dto)
        {
            var profile = await _context.ClientProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            _mapper.Map(dto, profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить профиль клиента
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// DELETE /api/clientprofiles/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClientProfile(Guid id)
        {
            var profile = await _context.ClientProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            _context.ClientProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
