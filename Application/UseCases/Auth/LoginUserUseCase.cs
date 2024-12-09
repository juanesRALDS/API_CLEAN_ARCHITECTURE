using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
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

    public LoginUserUseCase(IUserRepository userRepository, IOptions<JwtConfig> jwtConfig, IPasswordHasher passwordHasher)
    {

        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtConfig = jwtConfig.Value;
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // Generación del token
        return JwtHelper.GenerateToken(_jwtConfig, user, DateTime.UtcNow.AddHours(1));
    }


}

