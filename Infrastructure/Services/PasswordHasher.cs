using SagaAserhi.Domain.Interfaces;
using SagaAserhi.Domain.Interfaces.Utils;
using BCrypt.Net;

namespace SagaAserhi.Infrastructure.Services;
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

