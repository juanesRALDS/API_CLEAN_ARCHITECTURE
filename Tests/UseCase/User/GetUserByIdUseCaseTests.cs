using System;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.UseCases.Users;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using FluentAssertions;
using Moq;
using Xunit;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.Application.UseCases.Users;

public class GetUserByIdUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IGetUserByIdUseCase _useCase;

    public GetUserByIdUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _useCase = new GetUserByIdUseCase(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnUser_WhenUserIdIsValid()
    {
        // Arrange
        string? userId = "validUserId";
        User user = new()
        { 
            Id = userId, 
            Name = "Test User", 
            Email = "user1@example.com" 
        };
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId))
            .ReturnsAsync(user);

        // Act
        UserDto? result = await _useCase.Execute(userId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Name.Should().Be("Test User");
        result.Email.Should().Be("user1@example.com");
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentNullException_WhenUserIdIsNull()
    {
        // Arrange
        string? userId = null;

        // Act
        Func<Task> act = () => _useCase.Execute(userId!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("*User ID cannot be null*");
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenUserIdIsEmpty()
    {
        // Arrange
        string? userId = string.Empty;

        // Act
        Func<Task> act = () =>  _useCase.Execute(userId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*User ID cannot be empty*");
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        string? userId = "nonExistentUserId";
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        Func<Task> act = () => _useCase.Execute(userId);

        //assert 
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("user not found");
    }
}