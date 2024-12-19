using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.Auth.IAuthUsecases
{
    public interface IGetUserByTokenUseCase
    {
        Task<UserDto?> Execute(string tokens);
    }
}