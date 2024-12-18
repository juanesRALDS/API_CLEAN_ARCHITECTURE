using System.Security.Claims;

namespace SagaAserhi.Domain.Interfaces.Auth;
public interface ITokenService
{
    string? ValidateToken(string tokens);
    ClaimsPrincipal? ValidateTokenAndGetPrincipal(string tokens); 
}
