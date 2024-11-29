using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;

namespace api_completa_mongodb_net_6_0.Infrastructure.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly JwtConfig _jwtConfig;

    public PasswordResetService(IUserRepository userRepository, IPasswordResetTokenRepository passwordResetTokenRepository, JwtConfig jwtConfig)
    {
        _userRepository = userRepository;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> GeneratePasswordResetToken(string email)
    {
        // Verifica si el correo existe en la base de datos
        User? user = await _userRepository.GetByEmailAsync(email) ?? throw new Exception("El correo no está registrado.");

        // Genera el token JWT
        DateTime expiration = DateTime.UtcNow.AddHours(1);
        Claim[]? claims = new[]
        {
                new Claim("id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };
        string token = JwtHelper.GenerateToken(_jwtConfig.SecretKey, _jwtConfig.Issuer, user, expiration);

        // Guarda el token en la colección `PasswordResetTokens`
        PasswordResetToken resetToken = new()
        {
            UserId = user.Id,
            Token = token,
            Expiration = expiration
        };
        await _passwordResetTokenRepository.InsertAsync(resetToken);

        // Retorna la URL de restauración
        return $"https://yourfrontendapp.com/reset-password?token={token}";
    }
}

