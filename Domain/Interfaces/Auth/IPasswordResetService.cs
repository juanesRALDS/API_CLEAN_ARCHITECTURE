using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Application.Interfaces
{
    public interface IPasswordResetService
    {
        Task<string> GenerateResetTokenAsync(string email);
    }
}
