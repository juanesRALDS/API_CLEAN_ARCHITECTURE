// Application/UseCases/GetUserByTokenUseCase.cs
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Services;


namespace api_completa_mongodb_net_6_0.Application.UseCases.Auth
{
    public class GetUserByTokenUseCase
    {
        private readonly ITokenService _tokenServices;
        private readonly IUserRepository _userRepository;


        public GetUserByTokenUseCase(ITokenService tokenServices, IUserRepository userRepository)
        {
            _tokenServices = tokenServices;
            _userRepository = userRepository;
        }


        public async Task<UserDto?> Login(string tokens)
        {
            string? userId = _tokenServices.ValidateToken(tokens);
            if (userId == null) return null;

            Domain.Entities.User? user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            // Map the User entity to a UserDto
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                // Add other properties as needed
            };
        }
    }
}
