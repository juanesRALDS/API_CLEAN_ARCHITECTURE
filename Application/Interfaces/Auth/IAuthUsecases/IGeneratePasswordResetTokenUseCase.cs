using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases
{
    public interface IGeneratePasswordResetTokenUseCase
    {
        Task<string> Execute(string email);
    }
}