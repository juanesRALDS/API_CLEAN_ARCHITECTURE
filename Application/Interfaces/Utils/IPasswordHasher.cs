using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Domain.Interfaces.Utils
{
    public interface IPasswordHasher
    {
        string HashPassword(string Password);
        bool VerifyPassword(string password, string hashedPassword);

    }
}

