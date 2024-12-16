using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;
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
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id), "Id cannot be null");
        }
        if (id == string.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        await _userRepository.DeleteUser(id);
    }
}

