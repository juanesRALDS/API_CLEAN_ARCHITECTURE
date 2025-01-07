using SagaAserhi.Application.DTO;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Application.Interfaces.IRepository;


namespace SagaAserhi.Application.UseCases.Users;
public class GetAllUsersUseCase : IGetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Execute(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        List<User>? users = await _userRepository.GetAllUser(pageNumber, pageSize)
            ?? throw new InvalidOperationException("The repository returned null users list.");

        return users.Select(ToUserDto).ToList();
    }

    private static UserDto ToUserDto(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User entity cannot be null.");

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

