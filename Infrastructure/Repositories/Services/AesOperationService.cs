using System.Security.Cryptography;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Interfaces;


namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class AesOperationService : IEncryptionServices
    {
        public string EncryptPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var hashedInput = EncryptPassword(inputPassword);
            return hashedInput == hashedPassword;
        }
    }
}