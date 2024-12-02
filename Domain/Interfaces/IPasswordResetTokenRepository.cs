using api_completa_mongodb_net_6_0.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task SaveTokenAsync(Token tokens);
        Task<Token> GetByTokenAsync(string Tokens);
        Task DeleteTokenAsync(string tokenValue);
    }
}
