using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases;

public class GetUserByTokenUseCaseTests
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetUserByTokenUseCase _useCase;

    public GetUserByTokenUseCaseTests()
    {
        _mockTokenService = new Mock<ITokenService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _useCase = new GetUserByTokenUseCase(_mockTokenService.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnUserDto_WhenTokenIsValidAndUserExists()
    {
        // Arrange
        string validToken = "validToken123";
        string userId = "user123";

        _mockTokenService.Setup(ts => ts.ValidateToken(validToken)).Returns(userId);

        User user = new User
        {
            Id = userId,
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);

        // Act
        UserDto? result = await _useCase.Execute(validToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenTokenIsInvalid()
    {
        // Arrange
        string invalidToken = "invalidToken123";

        _mockTokenService.Setup(ts => ts.ValidateToken(invalidToken)).Returns((string?)null);

        // Act
        UserDto? result = await _useCase.Execute(invalidToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        string validToken = "validToken123";
        string userId = "user123";

        _mockTokenService.Setup(ts => ts.ValidateToken(validToken)).Returns(userId);
        _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        UserDto? result = await _useCase.Execute(validToken);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenTokenValidationFails()
    {
        // Arrange
        string token = "tokenWithError";

        _mockTokenService.Setup(ts => ts.ValidateToken(token)).Throws(new Exception("Token validation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(token));
    }
}
