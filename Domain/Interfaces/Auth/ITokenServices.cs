using System.Security.Claims;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
public interface ITokenService
{
    string? ValidateToken(string tokens);
    ClaimsPrincipal? ValidateTokenAndGetPrincipal(string tokens); // Nuevo método
}
