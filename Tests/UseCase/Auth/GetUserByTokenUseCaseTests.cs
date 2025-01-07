using System;
using System.Security.Claims;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Domain.Entities;
using Moq;
using Xunit;
using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.Auth;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.Application.UseCases;

public class GetUserByTokenUseCaseTests
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IGetUserByTokenUseCase _useCase;

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

        List<Claim> claims = new()
        {
            new Claim("id", userId)
        };
        ClaimsPrincipal? claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Corregir el setup del mock
        _mockTokenService
            .Setup(ts => ts.ValidateTokenAndGetPrincipal(validToken))
            .Returns(claimsPrincipal);

        User? user = new()
        {
            Id = userId,
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        _mockUserRepository
            .Setup(repo => repo.GetUserById(userId))
            .ReturnsAsync(user);

        // Act
        UserDto? result = await _useCase.Execute(validToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenTokenValidationFails()
    {
        // Arrange
        string token = "tokenWithError";

        _mockTokenService
            .Setup(ts => ts.ValidateTokenAndGetPrincipal(token))
            .Throws(new Exception("Token validation failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(token));
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenTokenIsInvalid()
    {
        // Arrange
        string invalidToken = "invalidToken123";

        _mockTokenService
            .Setup(ts => ts.ValidateTokenAndGetPrincipal(invalidToken))
            .Returns((ClaimsPrincipal?)null);

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

        List<Claim>? claims = new List<Claim>
        {
            new Claim("id", userId)
        };
        ClaimsPrincipal? claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        _mockTokenService
            .Setup(ts => ts.ValidateTokenAndGetPrincipal(validToken))
            .Returns(claimsPrincipal);

        _mockUserRepository
            .Setup(repo => repo.GetUserById(userId))
            .ReturnsAsync((User?)null);

        // Act
        UserDto? result = await _useCase.Execute(validToken);

        // Assert
        Assert.Null(result);
    }
}
