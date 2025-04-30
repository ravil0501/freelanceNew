namespace freelanceNew.DTOModels.UsersDTO
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Строкой для читаемости (например: "Client")
        public DateTime CreatedAt { get; set; }
    }
}
