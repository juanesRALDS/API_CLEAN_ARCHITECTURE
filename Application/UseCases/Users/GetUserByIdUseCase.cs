using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;

namespace api_completa_mongodb_net_6_0.Application.UseCases.Users;
public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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

