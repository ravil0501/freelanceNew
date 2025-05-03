using Microsoft.AspNetCore.Mvc;
using freelanceNew.Services;
using freelanceNew.DTOModels;
using freelanceNew.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using freelanceNew.DTOModels.AuthenticationDto;
using freelanceNew.Models.Enums;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            ApplicationDbContext context,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                // Проверка уникальности email и username
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                    return BadRequest("Email already exists");

                if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                    return BadRequest("Username already taken");

                // Создание пользователя
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = dto.Email,
                    Username = dto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 13),
                    Role = dto.Role,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Создание соответствующего профиля
                if (dto.Role == UserRole.Client)
                {
                    user.ClientProfile = new ClientProfile
                    {
                        ClientId = user.UserId,
                        CompanyName = $"{dto.Username}'s Company",
                        Location = "Unknown"
                    };
                }
                else if (dto.Role == UserRole.Freelancer)
                {
                    user.FreelancerProfile = new FreelancerProfile
                    {
                        FreelancerId = user.UserId,
                        FullName = dto.Username,
                        HourlyRate = 0,
                        Location = "Unknown"
                    };
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Генерация токена
                var authResponse = _jwtService.GenerateToken(user);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password");

            return Ok(_jwtService.GenerateToken(user));
        }
    }
}
