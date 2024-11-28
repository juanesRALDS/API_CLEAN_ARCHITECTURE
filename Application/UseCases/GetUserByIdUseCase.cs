using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class GetUserByIdUseCase
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ExecuteAsync(string userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}
