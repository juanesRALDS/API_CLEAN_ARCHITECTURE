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

        // Obtener usuario existent
        User? existingUser = await _userRepository.GetUserById(id)
            ?? throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        // Crear usuario actualizado manteniendo datos existentes
        User? updatedUser = new()
        {
            Id = id,
            Name = updatedUserDto.Name ??  existingUser.Name,
            Email = updatedUserDto.Email ?? existingUser.Email
        };

        // Validar email solo si se proporciona
        if (updatedUserDto.Email != null && !Regex.IsMatch(updatedUserDto.Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
        {
            throw new FormatException("El email no es válido.");
        }

        await _userRepository.UpdateUser(id, updatedUser);

        return new UpdateUserResponseDto
        {
            Id = id,
            Name = updatedUser.Name,
            Email = updatedUser.Email,
        };
    }
}

