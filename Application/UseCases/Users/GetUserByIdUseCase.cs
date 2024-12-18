using SagaAserhi.Application.DTO;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Application.Interfaces;

namespace SagaAserhi.Application.UseCases.Users;
public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Execute(string userId)
    {
        if (userId == null)
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null");
            
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("user ID cannot be empty", nameof(userId));

        User? user = await _userRepository.GetUserById(userId) 
            ?? throw new InvalidOperationException("user not found");

        return new UserDtoResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

