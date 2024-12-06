using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace api_completa_mongodb_net_6_0.Infrastructure.Utils;

public static class JwtHelper
{
    private static string SecretKey => Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "TuClaveSecretaMuyLargaDe32Caracteres";
    private static string Issuer => "yourapp"; // Configura tu emisor
    private static string Audience => "yourapp"; // Configura tu audiencia

    public static string GenerateToken(string secretKey, string v, User user, DateTime expiration)
    {
        // Asegúrate de que la clave sea de al menos 32 caracteres
        if (SecretKey.Length < 32)
            throw new ArgumentException("La clave secreta debe tener al menos 32 caracteres.");

        // Configuración de la clave de firma
        SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        SigningCredentials? credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims personalizados para incluir información del usuario
        Claim[]? claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Identificador único del usuario
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Identificador del token
            new Claim("id", user.Id.ToString()), // ID del usuario como un claim personalizado
            new Claim("name", user.Name) // Nombre del usuario
        };

        // Creación del token JWT
        JwtSecurityToken? token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static JwtSecurityToken? DecodeJwtToken(string token)
    {
        JwtSecurityTokenHandler? handler = new JwtSecurityTokenHandler();

        // Verifica si el token es legible
        if (!handler.CanReadToken(token))
            return null; // Retorna null si el token no es válido

        try
        {
            // Decodifica el token JWT
            return handler.ReadJwtToken(token);
        }
        catch
        {
            return null; // Retorna null si ocurre algún error durante la decodificación
        }
    }


}
