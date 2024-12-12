using api_completa_mongodb_net_6_0.Domain.Interfaces;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using MongoApiDemo.Domain.Interfaces.Utils;

namespace api_completa_mongodb_net_6_0.Infrastructure.Utils;
    public class HashingHelper : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public  bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

