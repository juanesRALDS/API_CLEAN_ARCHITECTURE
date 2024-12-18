using SagaAserhi.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace SagaAserhi.Domain.Interfaces.Auth;
public interface IPasswordResetTokenRepository
{
    Task SaveToken(Token tokens);
    Task<Token> GetByToken(string Tokens);
    Task DeleteToken(string tokenValue);
}

