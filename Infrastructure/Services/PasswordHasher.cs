using api_completa_mongodb_net_6_0.Domain.Interfaces;
using BCrypt.Net;
using MongoApiDemo.Domain.Interfaces.Utils;

namespace api_completa_mongodb_net_6_0.Infrastructure.Services;
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }

