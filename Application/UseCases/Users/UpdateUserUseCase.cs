using System.Text.RegularExpressions;
using SagaAserhi.Application.DTO;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Utils;

namespace SagaAserhi.Application.UseCases.Users;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateUserUseCase(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UpdateUserResponseDto> Execute(string id, UpdateUserDto updatedUserDto)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("El ID del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(updatedUserDto.Name) ||
            string.IsNullOrWhiteSpace(updatedUserDto.Email) ||
            string.IsNullOrWhiteSpace(updatedUserDto.Password))
        {
            throw new ArgumentException("Todos los campos son obligatorios.");
        }

        if (!Regex.IsMatch(updatedUserDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new FormatException("El formato del correo electrónico no es válido.");
        }

        // Obtener usuario existente
        User? existingUser = await _userRepository.GetUserById(id)
            ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        // Hashear la nueva contraseña
        string hashedPassword = _passwordHasher.HashPassword(updatedUserDto.Password);



        User? updatedUser = new()
        {
            Id = id,
            Name = updatedUserDto.Name,
            Email = updatedUserDto.Email,
            Password = hashedPassword
        };

        await _userRepository.UpdateUser(id, updatedUser);

        return new UpdateUserResponseDto
        {
            Id = id,
            Name = updatedUserDto.Name,
            Email = updatedUserDto.Email,
        };
    }
}

