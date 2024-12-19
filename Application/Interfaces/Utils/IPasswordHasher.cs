using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.Utils;

public interface IPasswordHasher
{
    string HashPassword(string Password);
    bool VerifyPassword(string password, string hashedPassword);

}

