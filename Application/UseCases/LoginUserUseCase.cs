using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IAuthService _authService;

        public LoginUserUseCase(IAuthService authService)
        {
            _authService = authService?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<string> ExecuteAsync(LoginUserDto loginDto)
        {

            if(loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            if(string.IsNullOrWhiteSpace(loginDto.Email)) throw new ArgumentException("mail can not be empty");
            if(string.IsNullOrWhiteSpace(loginDto.Password)) throw new ArgumentException("Password cannot be empty");

            return await _authService.LoginAsync(loginDto);

        }

        public async Task RegisterAsync(CreateUserDto userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));
            if(string.IsNullOrWhiteSpace(userDto.Name)) throw new ArgumentException("Name cannot be empty");
            if(string.IsNullOrWhiteSpace(userDto.Email)) throw new ArgumentException("Email cannot be empty");
            if(string.IsNullOrWhiteSpace(userDto.Password)) throw new ArgumentException("Password cannot be empty.");
            
            await _authService.RegisterAsync(userDto);
        }
    }
}
