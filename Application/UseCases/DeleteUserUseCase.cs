using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Repositories;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class DeleteUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
