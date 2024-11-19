using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Infrastructure.Utils;
    public static class JwtHelper
    {
        public static string GenerateToken(User user)
        {
            // Obtén la clave secreta desde las variables de entorno o `appsettings.json`
            string? secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "TuClaveSecretaMuyLargaDe32Caracteres";
            
            // Asegúrate de que la clave sea de al menos 32 caracteres
            if (secretKey.Length < 32)
                throw new ArgumentException("La clave secreta debe tener al menos 32 caracteres.");

            SymmetricSecurityKey? key = new(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials? credentials = new(key, SecurityAlgorithms.HmacSha256);

             Claim[]? claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken? token = new(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

