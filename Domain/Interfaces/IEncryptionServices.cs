using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces

{
    public interface IEncryptionServices
    {
        string EncryptPassword(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
        
    }
}