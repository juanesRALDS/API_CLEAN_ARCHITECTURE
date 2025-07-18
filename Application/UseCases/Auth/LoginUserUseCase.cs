using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Utils;
using SagaAserhi.Infrastructure.Config;
using SagaAserhi.Infrastructure.Utils;
using SagaAserhi.Application.DTO.Auth;
using Microsoft.Extensions.Options;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.Auth;

public class LoginUserUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserUseCase(
        IUserRepository userRepository,
        IOptions<JwtConfig> jwtConfig,
        IPasswordHasher passwordHasher
        )
    {
        _userRepository = userRepository;
        _jwtConfig = jwtConfig.Value;
        _passwordHasher = passwordHasher;
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


        User? user = await _userRepository.GetUserByEmail(loginDto.Email);
        if (user is null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        return JwtHelper.GenerateToken(_jwtConfig, user, DateTime.UtcNow.AddHours(1));
    }
}

