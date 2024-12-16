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
        _passwordHasher = passwordHasher ?? throw new ArgumentOutOfRangeException(nameof(passwordHasher)); 
        _jwtConfig = jwtConfig.Value;
    }

    public async Task<string> Execute(CreateUserDto? userDto)
    {
        if (userDto == null)
            throw new ArgumentNullException(nameof(userDto));

        if (string.IsNullOrWhiteSpace(userDto.Name))
            throw new ArgumentException("the name cannot be empty or null", nameof(userDto.Name));

        if (string.IsNullOrWhiteSpace(userDto.Email))
            throw new ArgumentNullException("the email cannot be empty or null", nameof(userDto.Email));

        if (string.IsNullOrWhiteSpace(userDto.Password))
            throw new ArgumentException("the password cannot be empty or null", nameof(userDto.Password));


        User? existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        if (!IsValidEmail(userDto.Email))
        {
            throw new FormatException("The email format is invalid.");
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
