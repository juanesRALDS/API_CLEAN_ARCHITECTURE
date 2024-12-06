using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;
public class RegisterUseCase
{
    private readonly IAuthService _authServices;

    public RegisterUseCase(IAuthService authServices)
    {
        _authServices = authServices ?? throw new ArgumentOutOfRangeException(nameof(authServices));
    }

    public async Task Register(CreateUserDto userDto)
    {
        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("Name cannot be empty or whitespace.", nameof(userDto.Name));

        if (string.IsNullOrWhiteSpace(userDto.Email))
            throw new ArgumentNullException("Email cannot be empty or whitespace.", nameof(userDto.Email));

        if (string.IsNullOrWhiteSpace(userDto.Password))
            throw new ArgumentException("Password cannot be empty or whitespace.", nameof(userDto.Password));

        await _authServices.Register(userDto);
    }


}
