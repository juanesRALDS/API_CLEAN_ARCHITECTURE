using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;

namespace MongoApiDemo.Domain.Interfaces.Auth.IAuthUsecases
{
    public interface ILoginUseCase
    {
        Task<string> Login (LoginUserDto loginDto);
    }
}