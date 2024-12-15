using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth;
public class RegisterUseCase : IRegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfig _jwtConfig;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUseCase(IUserRepository userRepository, IOptions<JwtConfig> jwtConfig, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository ?? throw new ArgumentOutOfRangeException(nameof(userRepository));
        _passwordHasher = passwordHasher;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> Execute(CreateUserDto userDto)
    {
        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("El nombre no puede estar vacío o nulo.", nameof(userDto.Name));

        if (string.IsNullOrWhiteSpace(userDto.Email))
            throw new ArgumentNullException("el correo no puede ser nulo", nameof(userDto.Email));

        if (string.IsNullOrWhiteSpace(userDto.Password))
            throw new ArgumentException("la contraseña no puede  estar vacia o ser nula ", nameof(userDto.Password));


        User? existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Ya existe un usuario registrado con este correo.");
        }

        if (!IsValidEmail(userDto.Email))
        {
            throw new FormatException("El correo electrónico no tiene un formato válido.");
        }

        string? hashedPassword = _passwordHasher.HashPassword(userDto.Password);

        User? newUser = new()
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = hashedPassword
        };

        await _userRepository.CreateNewUser(newUser);

        return "User registered successfully";
    }
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        string? emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }
}
