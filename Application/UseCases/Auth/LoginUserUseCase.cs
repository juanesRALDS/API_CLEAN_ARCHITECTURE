using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using MongoApiDemo.Domain.Interfaces.Auth.IAuthUsecases;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;

public class LoginUserUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserUseCase( IUserRepository userRepository, IOptions<JwtConfig> jwtConfig, IPasswordHasher passwordHasher)
    {

        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtConfig = jwtConfig.Value;
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<string> Login(LoginUserDto loginDto)
    {
        if (loginDto == null)
            throw new ArgumentNullException(nameof(loginDto));

        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
            throw new ArgumentException("Email cannot be empty or whitespace.", nameof(loginDto.Email));
        }

        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
            throw new UnauthorizedAccessException("Credenciales inválidas");


        if (string.IsNullOrWhiteSpace(loginDto.Password))
            throw new ArgumentException("Password cannot be empty or whitespace.", nameof(loginDto.Password));

        return JwtHelper.GenerateToken(_jwtConfig.SecretKey, _jwtConfig.Issuer, user, DateTime.UtcNow.AddHours(1));
    }


}

