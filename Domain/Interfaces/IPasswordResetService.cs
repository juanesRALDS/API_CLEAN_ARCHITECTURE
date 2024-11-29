namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IPasswordResetService
    {
        Task<string> GeneratePasswordResetToken(string email);
    }
}
