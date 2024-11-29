using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task InsertAsync(PasswordResetToken token);
    }
}
