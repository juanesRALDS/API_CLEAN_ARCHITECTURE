using SagaAserhi.Domain.Interfaces;
using SagaAserhi.Domain.Interfaces.Utils;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace SagaAserhi.Infrastructure.Utils;
public class PasswordHasher : IPasswordHasher
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

