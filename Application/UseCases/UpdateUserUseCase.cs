using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(string id, UpdateUserDto updatedUserDto)
        {

            if (string.IsNullOrEmpty(updatedUserDto.Name) || 
                string.IsNullOrEmpty(updatedUserDto.Email) || 
                string.IsNullOrEmpty(updatedUserDto.Password))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }


            User? userEntity = new()
            {
                Id = id, 
                Name = updatedUserDto.Name,
                Email = updatedUserDto.Email,
                Password = updatedUserDto.Password
            };

            await _userRepository.UpdateAsync(id, userEntity);
        }
    }
}
