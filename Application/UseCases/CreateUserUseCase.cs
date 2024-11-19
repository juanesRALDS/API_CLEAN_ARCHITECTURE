using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
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
            string?  hashedPassword = _passwordHasher.HashPassword(dto.Password);

            User? user = new()
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = hashedPassword
            };

            await _userRepository.CreateAsync(user);
        }
    }
}
