using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.Auth;
public interface IPasswordResetTokenRepository
{
    Task SaveToken(Token tokens);
    Task<Token> GetByToken(string Tokens);
    Task DeleteToken(string tokenValue);
}

