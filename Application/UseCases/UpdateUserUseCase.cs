using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using api_completa_mongodb_net_6_0.Infrastructure.Services;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UpdateUserResponseDto> ExecuteAsync(string id, UpdateUserDto updatedUserDto)
        {

            string? hashedPassword = _passwordHasher.HashPassword(updatedUserDto.Password);

            if (string.IsNullOrEmpty(updatedUserDto.Name) ||
                string.IsNullOrEmpty(updatedUserDto.Email) ||
                string.IsNullOrEmpty(updatedUserDto.Password))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }


            // Validar existencia del usuario
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            }


            User? userEntity = new()
            {
                Id = id,
                Name = updatedUserDto.Name,
                Email = updatedUserDto.Email,
                Password = hashedPassword
            };

            UpdateUserResponseDto updateUserResponseDto = new()
            {
                Id = id,
                Name = updatedUserDto.Name,
                Email = updatedUserDto.Email
            };

            await _userRepository.UpdateAsync(id, userEntity);

            

            return updateUserResponseDto;   

        }
    }
}
