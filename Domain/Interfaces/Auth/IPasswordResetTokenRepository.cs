using api_completa_mongodb_net_6_0.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
public interface IPasswordResetTokenRepository
{
    Task SaveToken(Token tokens);
    Task<Token> GetByToken(string Tokens);
    Task DeleteToken(string tokenValue);
}

