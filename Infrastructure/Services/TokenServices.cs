using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api_completa_mongodb_net_6_0.Infrastructure.Services;

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
