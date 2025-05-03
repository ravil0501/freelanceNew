using freelanceNew.DTOModels.AuthenticationDto;
using freelanceNew.Models;

namespace freelanceNew.Services
{
    public interface IJwtService
    {
        AuthResponseDto GenerateToken(User user);
    }
}
