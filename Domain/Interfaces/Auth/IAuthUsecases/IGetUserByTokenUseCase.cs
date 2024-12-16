using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases
{
    public interface IGetUserByTokenUseCase
    {
        Task<UserDto?> Execute(string tokens);
    }
}