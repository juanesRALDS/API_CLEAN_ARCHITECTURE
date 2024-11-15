using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IAuthService _authService;

        public LoginUserUseCase(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> ExecuteAsync(LoginUserDto loginDto)
        {
            return await _authService.LoginAsync(loginDto);
        }

        public async Task RegisterAsync(CreateUserDto userDto)
        {
            await _authService.RegisterAsync(userDto);
        }
    }
}
