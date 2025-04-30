using Microsoft.AspNetCore.Mvc;
using freelanceNew.DTOModels.ContractsDto;
using freelanceNew.Models;
using freelanceNew.Models.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContractsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractDto>>> GetContracts()
        {
            var contracts = await _context.Contracts.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ContractDto>>(contracts));
        }

        // GET: api/contracts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ContractDto>> GetContract(Guid id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            return Ok(_mapper.Map<ContractDto>(contract));
        }

        // POST: api/contracts
        [HttpPost]
        public async Task<ActionResult<ContractDto>> CreateContract(CreateContractDto dto)
        {
            if (!Enum.TryParse<ContractStatus>(dto.Status, out var status))
                return BadRequest("Invalid contract status.");

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

            var resultDto = _mapper.Map<ContractDto>(contract);
            return CreatedAtAction(nameof(GetContract), new { id = contract.ContractId }, resultDto);
        }

        // PUT: api/contracts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContract(Guid id, UpdateContractDto dto)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

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
        public async Task<IActionResult> DeleteContract(Guid id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
