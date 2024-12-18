using System;
using System.Threading.Tasks;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Domain.Interfaces.Auth;
using Microsoft.IdentityModel.Tokens;
using SagaAserhi.Domain.Interfaces.Utils;
using Moq;
using Xunit;
using FluentAssertions;
using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces;

namespace SagaAserhi.Tests.UseCase.Auth;
public class UpdatePasswordUseCaseTests
{
    private readonly Mock<IPasswordResetTokenRepository> _mockTokenRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IUpdatePasswordUseCase _useCase;
    private readonly Mock<IPasswordHasher> _mockPasswordHelper;

    public UpdatePasswordUseCaseTests()
    {
        _mockTokenRepository = new Mock<IPasswordResetTokenRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHelper = new Mock<IPasswordHasher>();
        _useCase = new UpdatePasswordUseCase(_mockTokenRepository.Object, _mockUserRepository.Object, _mockPasswordHelper.Object);
    }

    [Fact]
    public async Task WhenTokenIsValid()
    {
        Mock<IPasswordResetTokenRepository>? mockTokenRepository = new();
        Mock<IUserRepository>? mockUserRepository = new();
        Mock<IPasswordHasher>? mockPasswordHasher = new();

        // Arrange
        string token = "valid-token";
        string newPassword = "NewPassword123";
        string hashedPassword = "hashed-password";

        Token? validToken = new()
        {
            Tokens = token,
            Expiration = DateTime.UtcNow.AddMinutes(30),
            UserId = "userId"
        };

        mockTokenRepository
            .Setup(repo => repo.GetByToken(token))
            .ReturnsAsync(validToken);

        mockUserRepository
            .Setup(repo => repo.GetUserById("userId"))
            .ReturnsAsync(new User { Id = "userId" });

        mockPasswordHasher
            .Setup(hasher => hasher.HashPassword(newPassword))
            .Returns(hashedPassword);

        UpdatePasswordUseCase useCase = new(
            mockTokenRepository.Object,
            mockUserRepository.Object,
            mockPasswordHasher.Object
            );

        // Act
        bool result = await useCase.Execute(token, newPassword);

        // Assert
        Assert.True(result);
        mockUserRepository.Verify(repo => repo.UpdatePassword("userId", hashedPassword), Times.Once);
        mockTokenRepository.Verify(repo => repo.DeleteToken(token), Times.Once);
    }

    [Fact]
    public async Task WhenTokenNotFound()
    {

        // Arrange
        string token = "invalid-token";
        _mockTokenRepository.Setup(repo => repo.GetByToken(token))
            .ReturnsAsync((Token?)null!);

        //act
        Func<Task> act = async () => await _useCase.Execute(token, "NewPassword123");

        // Act & Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Token not found or is null");
    }

    [Fact]
    public async Task WhenTokenIsExpired()
    {
        // Arrange
        string token = "expired-token";
        Token? expiredToken = new()
        {
            Tokens = token,
            Expiration = DateTime.UtcNow.AddMinutes(-10),
            UserId = "user-id"
        };

        _mockTokenRepository
            .Setup(repo => repo.GetByToken(token))
            .ReturnsAsync(expiredToken);

        _mockUserRepository
            .Setup(repo => repo.GetUserById(expiredToken.UserId))
            .ReturnsAsync(new User { Id = expiredToken.UserId });

        // Act & Assert
         Func<Task> act = async () => await _useCase.Execute(token, "newPassword123");

        await act.Should().ThrowAsync<SecurityTokenExpiredException>()
            .WithMessage($"Token has expired, expiration: {expiredToken.Expiration}, current time: {DateTime.UtcNow}");
    }

    [Fact]
    public async Task WhenUserNotFound()
    {
        // Arrange
        string token = "valid-token";

        Token? storedToken = new()
        {
            Tokens = token,
            Expiration = DateTime.UtcNow.AddMinutes(10),
            UserId = "nonexistent-user-id"
        };

        _mockTokenRepository.Setup(repo => repo.GetByToken(token))
            .ReturnsAsync(storedToken);

        _mockUserRepository.Setup(repo => repo.GetUserById(storedToken.UserId))
            .ReturnsAsync((User?)null);

        //act
        Func<Task> act = async () => await _useCase.Execute(token, "NewPassword123");

        // Act & Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("The user associated with the token was not found");
    }
}
