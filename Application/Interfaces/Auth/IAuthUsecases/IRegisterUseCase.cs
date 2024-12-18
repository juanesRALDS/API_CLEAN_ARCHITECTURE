using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.Auth;

namespace SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases
{
    public interface IRegisterUseCase
    {
        Task<string> Execute(CreateUserDto? userDto);
    }
}