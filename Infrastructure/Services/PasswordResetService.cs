using api_completa_mongodb_net_6_0.Application.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;

namespace api_completa_mongodb_net_6_0.Application.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly JwtConfig _jwtConfig;

    public PasswordResetService(IUserRepository userRepository, IPasswordResetTokenRepository tokenRepository, JwtConfig jwtConfig)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> GenerateResetTokenAsync(string email)
    {
        // Validar si el usuario existe
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            throw new Exception("Usuario no encontrado con el correo proporcionado.");

        // Generar el token usando JwtHelper
        var expiration = DateTime.UtcNow.AddMinutes(30);
        var tokenValue = JwtHelper.GenerateToken(
            secretKey: Environment.GetEnvironmentVariable(_jwtConfig.SecretKey) ?? "TuClaveSecretaMuyLargaDe32Caracteres",
            v: string.Empty,
            user: user,
            expiration: expiration
        );

        // Crear el documento de token
        var token = new Token
        {
            Tokens = tokenValue,
            Expiration = expiration,
            UserId = user.Id
        };

        // Guardar el token en la base de datos
        await _tokenRepository.SaveTokenAsync(token);

        // Retornar la URL de restablecimiento
        return $"https://tu-dominio.com/reset-password?token={tokenValue}";
    }
}
