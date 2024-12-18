using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.UseCase.Users;

public class UpdateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly IUpdateUserUseCase _useCase;

    public UpdateUserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _useCase = new UpdateUserUseCase(_userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldUpdateUser_WhenDataIsValid()
    {
        // Arrange
        string? userId = "1";
        UpdateUserDto? updateUserDto = new() { Name = "UpdatedName", Email = "updated@example.com", Password = "newpassword" };
        User? user = new() { Id = userId, Name = "OldName", Email = "old@example.com", Password = "oldpassword" };

        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
        _passwordHasherMock.Setup(hasher => hasher.HashPassword(updateUserDto.Password)).Returns("hashedpassword");

        // Act
        await _useCase.Execute(userId, updateUserDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.UpdateUser(userId,It.Is<User>(u => 
            u.Id == userId && 
            u.Name == updateUserDto.Name &&
            u.Email == updateUserDto.Email && 
            u.Password == "hashedpassword")), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        UpdateUserDto? updateUserDto = new() { Name = "UpdatedName", Email = "updated@example.com", Password = "newpassword" };

        // Act
        Func<Task> act = async () => await _useCase.Execute("", updateUserDto);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenAnyFieldIsEmpty()
    {
        // Arrange
        string? userId = "1";
        UpdateUserDto? updateUserDto = new() { Name = "", Email = "updated@example.com", Password = "newpassword" };

        // Act
        Func<Task> act = async () => await _useCase.Execute(userId, updateUserDto);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    [Fact]
    public async Task Execute_ShouldThrowKeyNotFoundException_WhenUserNotFound()
    {
        // Arrange
        string? userId = "1";
        UpdateUserDto? updateUserDto = new() { Name = "UpdatedName", Email = "updated@example.com", Password = "newpassword" };

        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User?)null);

        // Act
        Func<Task> act = async () => await _useCase.Execute(userId, updateUserDto);

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }
}