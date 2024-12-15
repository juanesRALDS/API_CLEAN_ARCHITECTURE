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
        _userRepository = userRepository;
    }

    public async Task<UserDto> Execute(string userId)
    {
        User? user = await _userRepository.GetUserById(userId);
        if (user == null)
        {   
            throw new InvalidOperationException("User not found.");
        }
        return new UserDtoResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

