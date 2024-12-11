using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases
{
    public interface IGeneratePasswordResetTokenUseCase
    {
        Task<string> Execute(string email);
    }
}