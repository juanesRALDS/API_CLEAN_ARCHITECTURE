using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;

namespace api_completa_mongodb_net_6_0.Infrastructure.Services;

public class TokenServices : ITokenService
{
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.SecretKey)) // Reemplaza con tu clave secreta
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
