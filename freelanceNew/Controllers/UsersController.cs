using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using freelanceNew.Models;
using freelanceNew.DTOModels.UsersDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using freelanceNew.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace freelanceNew.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требует аутентификации для всех endpoints
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users?role=Admin&email=test@example.com
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] string role = null,
            [FromQuery] string email = null,
            [FromQuery] string username = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(role))
            {
                if (!Enum.TryParse<UserRole>(role, true, out var roleEnum))
                    return BadRequest("Invalid role value");

                query = query.Where(u => u.Role == roleEnum);
            }

            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.Username.Contains(username));

            var users = await query.ToListAsync();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous] // Разрешает регистрацию без аутентификации
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("User with this email already exists.");

            var user = _mapper.Map<User>(dto);
            user.UserId = Guid.NewGuid();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password); // Более безопасное хэширование
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, _mapper.Map<UserDto>(user));
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            var currentUserId = GetCurrentUserId(); // Получаем ID текущего пользователя
            if (id != currentUserId && !User.IsInRole("Admin")) // Только админ может редактировать других
                return Forbid();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _mapper.Map(dto, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Только для админов
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/Users/{id}/changepassword
        [HttpPatch("{id}/changepassword")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordDto dto)
        {
            var currentUserId = GetCurrentUserId();
            if (id != currentUserId) // Можно менять только свой пароль
                return Forbid();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
                return BadRequest("Incorrect current password.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            if(!User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var userIdClaim = User.FindFirst("sub") ??
                throw new UnauthorizedAccessException("User ID claim not found");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user ID format");

            return userId;
        }
    }
}
// GET /api/users
// GET /api/users?role=Client
// GET /api/users?email=test@
// GET /api/users?username=john&role=Freelancer
/*
  Возвращает:
  [
    {
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "username": "john_doe",
      "email": "john@example.com",
      "role": "Freelancer",
      "createdAt": "2023-05-20T10:30:00Z"
    }
  ]
*/
// GET /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
/*
  Возвращает:
  {
    "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "username": "admin",
    "email": "admin@example.com",
    "role": "Admin",
    "createdAt": "2023-01-15T08:00:00Z"
  }
*/
// POST /api/users
// Headers: Content-Type: application/json
/* Body:
{
    "username": "new_user",
  "email": "new@example.com",
  "password": "SecurePass123!",
  "role": "Client"
}*/
/*
  Возвращает 201 Created с данными нового пользователя
*/
// PUT /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
// Headers: 
//   Content-Type: application/json
//   Authorization: Bearer {your_jwt_token}
/* Body:
{
    "username": "updated_username",
  "email": "updated@example.com",
  "role": "Freelancer"
}*/
/*
  Возвращает 204 No Content
*/
// DELETE /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6
// Headers: Authorization: Bearer {admin_jwt_token}
/*
  Возвращает 204 No Content
*/
// PATCH /api/users/3fa85f64-5717-4562-b3fc-2c963f66afa6/changepassword
// Headers: 
//   Content-Type: application/json
//   Authorization: Bearer {your_jwt_token}
/* Body:
{
    "currentPassword": "oldPassword123",
  "newPassword": "NewSecurePass456!"
}*/
/*
  Возвращает 204 No Content
*/