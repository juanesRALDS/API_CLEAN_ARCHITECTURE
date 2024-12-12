using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.User;
using api_completa_mongodb_net_6_0.Infrastructure.Repositories;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Users;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserRepository _userRepository;

    public DeleteUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Execute(string id)
    {
        await _userRepository.DeleteAsync(id);
    }
}

