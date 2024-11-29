using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
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
        var expiration = DateTime.UtcNow.AddHours(2);

        // Llama a JwtHelper.GenerateToken con los parámetros correctos
        return JwtHelper.GenerateToken(
            _jwtConfig.SecretKey, // Clave secreta desde JwtHelper
            _jwtConfig.Issuer,    // Issuer desde JwtHelper
            user,                // Usuario
            expiration           // Fecha de expiración
        );
    }
    public string? ValidateToken(string token)
    {
        // Usa directamente el método estático de JwtHelper
        var jwtToken = JwtHelper.DecodeJwtToken(token);
        if (jwtToken == null)
        {
            return null; // Retorna null si el token es inválido
        }

        // Obtén el valor del claim con el tipo "id"
        return jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
    }

    public ClaimsPrincipal? ValidateTokenAndGetPrincipal(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        try
        {
            // Configura la validación del token (ajusta según tu lógica)
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)) // Reemplaza con tu clave secreta
            };

            var principal = handler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null; // Retorna null si la validación falla
        }
    }

}
