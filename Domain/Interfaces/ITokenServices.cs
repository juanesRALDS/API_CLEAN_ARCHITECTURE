using System.Security.Claims;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface ITokenService
    {
        string? ValidateToken(string token);
        ClaimsPrincipal? ValidateTokenAndGetPrincipal(string token); // Nuevo método
    }
}