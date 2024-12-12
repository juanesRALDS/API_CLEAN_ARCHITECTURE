using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoApiDemo.Domain.Interfaces.Utils
{
    public interface IPasswordHasher
    {
        string HashPassword(String Password);
        bool VerifyPassword(string password, string hashedPassword);

    }
}

