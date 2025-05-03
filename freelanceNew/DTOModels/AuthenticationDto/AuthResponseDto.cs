namespace freelanceNew.DTOModels.AuthenticationDto
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
    }
}
