using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Infrastructure.Repositories;

namespace SagaAserhi.Application.UseCases.Users;
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

