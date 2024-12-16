using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;
using FluentAssertions;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Users;

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
        var userId = "validUserId";
        var user = new User 
        { 
            Id = userId, 
            Name = "Test User", 
            Email = "user1@example.com" 
        };
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _useCase.Execute(userId);

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
        var userId = string.Empty;

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
        var userId = "nonExistentUserId";
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        Func<Task> act = () => _useCase.Execute(userId);

        //assert 
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("user not found");
    }
}