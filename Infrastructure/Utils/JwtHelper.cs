using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Infrastructure.Config; 
using Microsoft.IdentityModel.Tokens;

namespace api_completa_mongodb_net_6_0.Infrastructure.Utils;

public static class JwtHelper
{
    public static string GenerateToken(JwtConfig jwtConfig, User user, DateTime expiration)
    {
        
        if (jwtConfig == null)
            throw new ArgumentNullException(nameof(jwtConfig));
        if (string.IsNullOrEmpty(jwtConfig.SecretKey) || jwtConfig.SecretKey.Length < 32)
            throw new ArgumentException("La clave secreta debe tener al menos 32 caracteres.", nameof(jwtConfig.SecretKey));
        if (string.IsNullOrEmpty(jwtConfig.Issuer))
            throw new ArgumentException("El emisor (Issuer) no puede estar vacío.", nameof(jwtConfig.Issuer));
        if (string.IsNullOrEmpty(jwtConfig.Audience))
            throw new ArgumentException("La audiencia (Audience) no puede estar vacía.", nameof(jwtConfig.Audience));


        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtConfig.SecretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);


        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new Claim("id", user.Id.ToString()), 
            new Claim("name", user.Name) 
        };


        JwtSecurityToken token = new(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static JwtSecurityToken? DecodeJwtToken(string token)
    {
        JwtSecurityTokenHandler handler = new();

        if (!handler.CanReadToken(token))
            return null;

        try
        {
            return handler.ReadJwtToken(token);
        }
        catch
        {
            return null;
        }
    }
}
