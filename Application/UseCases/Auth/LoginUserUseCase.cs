using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class LoginUserUseCase
{
    private readonly IAuthService _authService;

    public LoginUserUseCase(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    public async Task<string> Login(LoginUserDto loginDto)
    {
        if (loginDto == null)
            throw new ArgumentNullException(nameof(loginDto));

        if (string.IsNullOrWhiteSpace(loginDto.Email))
            throw new ArgumentException("Email cannot be empty or whitespace.", nameof(loginDto.Email));

        if (string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("Password cannot be empty or whitespace.", nameof(loginDto.Password));

        return await _authService.LoginAsync(loginDto);
    }


}

