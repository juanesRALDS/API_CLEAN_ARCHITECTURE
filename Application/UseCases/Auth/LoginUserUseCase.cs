using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class LoginUserUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IPasswordHasher _passwordHasher;

    private readonly IHttpContextAccessor _httpContextAccessor;
    public LoginUserUseCase(
        IUserRepository userRepository,
        IOptions<JwtConfig> jwtConfig,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContentAcecesor
        )
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtConfig = jwtConfig.Value;
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _httpContextAccessor = httpContentAcecesor ?? throw new ArgumentNullException(nameof(httpContentAcecesor));
    }

    public async Task<string> Execute(LoginUserDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Email))
        {
            throw new ArgumentException("El email no puede estar vacío.", nameof(loginDto.Email));
        }

        if (string.IsNullOrWhiteSpace(loginDto.Password))
        {
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(loginDto.Password));
        }

        // Validación de usuario en la base de datos
        User? user = await _userRepository.GetUserByEmail(loginDto.Email);
        if (user is null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        return JwtHelper.GenerateToken(_jwtConfig, user, DateTime.UtcNow.AddHours(1));
    }
}

