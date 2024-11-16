namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
    }
}
