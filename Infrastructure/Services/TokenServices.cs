using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Domain.Interfaces.Auth;
using SagaAserhi.Infrastructure.Config;
using SagaAserhi.Infrastructure.Utils;
using Microsoft.Extensions.Options;

namespace SagaAserhi.Infrastructure.Services;

public class TokenServices : ITokenService
{
    private readonly JwtConfig _jwtConfig;

    public TokenServices(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }
    public string GenerateUserToken(User user)
    {
        DateTime expiration = DateTime.UtcNow.AddHours(2);

        return JwtHelper.GenerateToken(
            jwtConfig: _jwtConfig,
            user: user,
            expiration: expiration
        );
    }
    public string? ValidateToken(string token)
    {

        JwtSecurityToken? jwtToken = JwtHelper.DecodeJwtToken(token);
        if (jwtToken == null)
        {
            return null;
        }


        return jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
    }

    public ClaimsPrincipal? ValidateTokenAndGetPrincipal(string token)
    {
        JwtSecurityTokenHandler? handler = new();

        if (string.IsNullOrEmpty(_jwtConfig.SecretKey))
        {
            throw new InvalidOperationException("La clave secreta para JWT no está configurada.");
        }

        try
        {
            TokenValidationParameters? validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey))
            };

            ClaimsPrincipal? principal = handler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

}

