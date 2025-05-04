using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels.ContractsDto;
using freelanceNew.Models;
using freelanceNew.Models.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContractsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/contracts?status=Active&freelancerId=xxx&clientId=xxx
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContracts(
            [FromQuery] string status = null,
            [FromQuery] Guid? freelancerId = null,
            [FromQuery] Guid? clientId = null,
            [FromQuery] Guid? jobId = null)
        {
            var query = _context.Contracts.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (!Enum.TryParse<ContractStatus>(status, true, out var statusEnum))
                    return BadRequest("Invalid contract status");
                query = query.Where(c => c.Status == statusEnum);
            }

            if (freelancerId.HasValue)
                query = query.Where(c => c.FreelancerId == freelancerId.Value);

            if (clientId.HasValue)
                query = query.Where(c => c.ClientId == clientId.Value);

            if (jobId.HasValue)
                query = query.Where(c => c.JobId == jobId.Value);

            var contracts = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ContractDto>>(contracts));
        }

        // GET: api/contracts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractDetailsDto>> GetContract(Guid id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Job)
                .Include(c => c.FreelancerProfile)  // Было Freelancer
                .Include(c => c.ClientProfile)      // Было Client
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null) return NotFound();

            return Ok(_mapper.Map<ContractDetailsDto>(contract));
        }

        // POST: api/contracts
        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
        public async Task<ActionResult<ContractDto>> CreateContract(CreateContractDto dto)
        {
            var currentUserId = GetCurrentUserId();

            // Проверка что текущий пользователь - клиент из контракта
            if (dto.ClientId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            if (!Enum.TryParse<ContractStatus>(dto.Status, out var status))
                return BadRequest("Invalid contract status.");

            // Проверка существования работы и фрилансера
            if (!await _context.Jobs.AnyAsync(j => j.JobId == dto.JobId))
                return BadRequest("Job not found");

            if (!await _context.Users.AnyAsync(u => u.UserId == dto.FreelancerId && u.Role == UserRole.Freelancer))
                return BadRequest("Freelancer not found");

            var contract = new Contract
            {
                ContractId = Guid.NewGuid(),
                JobId = dto.JobId,
                FreelancerId = dto.FreelancerId,
                ClientId = dto.ClientId,
                StartDate = DateTime.UtcNow,
                AgreedRate = dto.AgreedRate,
                Status = status
            };

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContract),
                new { id = contract.ContractId },
                _mapper.Map<ContractDto>(contract));
        }

        // PUT: api/contracts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(Guid id, UpdateContractDto dto)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            // Только участники контракта или админ могут изменять
            if (contract.ClientId != currentUserId &&
                contract.FreelancerId != currentUserId &&
                !User.IsInRole("Admin"))
                return Forbid();

            if (!Enum.TryParse<ContractStatus>(dto.Status, out var status))
                return BadRequest("Invalid contract status.");

            contract.AgreedRate = dto.AgreedRate;
            contract.Status = status;
            contract.EndDate = dto.EndDate ?? contract.EndDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/contracts/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteContract(Guid id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/contracts/{id}/complete
        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> CompleteContract(Guid id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            if (contract.ClientId != currentUserId && !User.IsInRole("Admin"))
                return Forbid();

            contract.Status = ContractStatus.Completed;
            contract.EndDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            // 1. Проверка аутентификации пользователя
            if (!User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            // 2. Поиск claim с идентификатором пользователя
            var userIdClaim = User.FindFirst("sub") ??
                throw new UnauthorizedAccessException("User ID claim not found in token");

            // 3. Валидация формата идентификатора
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException($"Invalid user ID format: {userIdClaim.Value}");

            return userId;
        }
    }
}
// GET /api/contracts
// GET /api/contracts?status=Active
// GET /api/contracts?freelancerId=3fa85f64-5717-4562-b3fc-2c963f66afa6
// GET /api/contracts?clientId=3fa85f64-5717-4562-b3fc-2c963f66afa6&status=Completed
/*
  Заголовки:
    Authorization: Bearer {your_jwt_token}
  
  Возвращает:
  [
    {
      "contractId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "freelancerId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "clientId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
      "startDate": "2023-05-20T10:30:00Z",
      "endDate": null,
      "agreedRate": 50.00,
      "status": "Active"
    }
  ]
*/

// GET /api/contracts/3fa85f64-5717-4562-b3fc-2c963f66afa6
/*
  Заголовки:
    Authorization: Bearer {your_jwt_token}
  
  Возвращает:
  {
    "contractId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "freelancerId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
    "clientId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
    "startDate": "2023-05-20T10:30:00Z",
    "endDate": null,
    "agreedRate": 50.00,
    "status": "Active",
    "job": {
      "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
      "title": "Разработка сайта",
      "description": "Нужен сайт для компании"
    },
    "freelancerProfile": {
      "freelancerId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
      "fullName": "Иван Петров",
      "hourlyRate": 30.00
    },
    "clientProfile

// POST /api/contracts
/*
  Заголовки:
    Authorization: Bearer {client_or_admin_token}
    Content-Type: application/json
  
  Тело запроса:
  {
    "jobId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "freelancerId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
    "clientId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
    "agreedRate": 50.00,
    "status": "Active"
  }
  
  Возвращает 201 Created с данными нового контракта
*/

// PUT /api/contracts/3fa85f64-5717-4562-b3fc-2c963f66afa6
/*
  Заголовки:
    Authorization: Bearer {participant_or_admin_token}
    Content-Type: application/json
  
  Тело запроса:
  {
    "agreedRate": 55.00,
    "status": "Active",
    "endDate": "2023-06-20T00:00:00Z"
  }
  
  Возвращает 204 No Content
*/

// DELETE /api/contracts/3fa85f64-5717-4562-b3fc-2c963f66afa6
/*
  Заголовки:
    Authorization: Bearer {admin_token}
  
  Возвращает 204 No Content
*/

// PATCH /api/contracts/3fa85f64-5717-4562-b3fc-2c963f66afa6/complete
/*
  Заголовки:
    Authorization: Bearer {client_or_admin_token}
  
  Возвращает 204 No Content
*/
