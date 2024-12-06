using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task ExecuteAsync(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentNullException(nameof(dto.Name), "El nombre no puede estar vacío o nulo.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ArgumentException("La contraseña no puede estar vacía o nula.", nameof(dto.Password));
        }

        if (!IsValidEmail(dto.Email))
        {
            throw new FormatException("El correo electrónico no tiene un formato válido.");
        }

        string hashedPassword = _passwordHasher.HashPassword(dto.Password);

        User user = new()
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = hashedPassword
        };

        await _userRepository.CreateAsync(user);
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        // Expresión regular básica para validar correos electrónicos
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }
}

