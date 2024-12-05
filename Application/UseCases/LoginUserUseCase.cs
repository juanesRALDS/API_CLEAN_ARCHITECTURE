using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IAuthService _authService;

        public LoginUserUseCase(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<string> ExecuteAsync(LoginUserDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            if (string.IsNullOrWhiteSpace(loginDto.Email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(loginDto.Email));

            if (string.IsNullOrWhiteSpace(loginDto.Password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(loginDto.Password));

            return await _authService.LoginAsync(loginDto);
        }

        public async Task RegisterAsync(CreateUserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            if (string.IsNullOrWhiteSpace(userDto.Name))
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(userDto.Name));

            if (string.IsNullOrWhiteSpace(userDto.Email))
                throw new ArgumentNullException("Email cannot be empty or whitespace.", nameof(userDto.Email));

            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(userDto.Password));

            await _authService.RegisterAsync(userDto);
        }

    }
}
