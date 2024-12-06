using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
public interface IAuthService
{
    Task<string> LoginAsync(LoginUserDto loginDto);
    Task Register(CreateUserDto userDto);
}

