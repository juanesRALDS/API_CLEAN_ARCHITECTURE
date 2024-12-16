using FluentAssertions;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;

namespace api_completa_mongodb_net_6_0.Tests.UseCase.Users;

public class GetAllUsersUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IGetAllUsersUseCase _useCase;

    public GetAllUsersUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _useCase = new GetAllUsersUseCase(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = "1", Name = "User1", Email = "user1@example.com" },
            new User { Id = "2", Name = "User2", Email = "user2@example.com" }
        };
        _userRepositoryMock.Setup(repo => repo.GetAllUser(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);

        // Act
        var result = await _useCase.Execute(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("User1");
        result[0].Email.Should().Be("user1@example.com");
        result[1].Name.Should().Be("User2");
        result[1].Email.Should().Be("user2@example.com");
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenPageNumberIsInvalid()
    {
        // Act
        var act = () => _useCase.Execute(0, 10);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Page number must be greater than 0*");
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenPageSizeIsInvalid()
    {
        // Act
        var act = () => _useCase.Execute(1, 0);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Page size must be greater than 0*");
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenRepositoryReturnsNull()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetAllUser(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((List<User>?)null!);

        // Act
        var act = () => _useCase.Execute(1, 10);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*repository returned null*");
    }
}