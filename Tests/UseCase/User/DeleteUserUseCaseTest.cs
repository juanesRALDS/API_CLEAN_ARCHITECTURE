using FluentAssertions;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Users;

public class DeleteUserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly DeleteUserUseCase _useCase;

    public DeleteUserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _useCase = new DeleteUserUseCase(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldDeleteUser_WhenIdIsValid()
    {
        // Arrange
        var userId = "validUserId";
        _userRepositoryMock.Setup(repo => repo.DeleteUser(userId))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.Execute(userId);

        // Assert
        _userRepositoryMock.Verify(repo => repo.DeleteUser(userId), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentNullException_WhenIdIsNull()
    {
        // Arrange
        string? userId = null;

        // Act
        Func<Task> act = () => _useCase.Execute(userId!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*Id cannot be null*");
        _userRepositoryMock.Verify(repo => repo.DeleteUser(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var userId = string.Empty;

        // Act
        Func<Task> act = () => _useCase.Execute(userId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Id cannot be empty*");
        _userRepositoryMock.Verify(repo => repo.DeleteUser(It.IsAny<string>()), Times.Never);
    }
}