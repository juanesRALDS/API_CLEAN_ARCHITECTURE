using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace api_completa_mongodb_net_6_0.Application.UseCases.Users;
public class GetAllUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<List<UserDto>> ExecuteAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        var users = await _userRepository.GetAllAsync(pageNumber, pageSize)
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

