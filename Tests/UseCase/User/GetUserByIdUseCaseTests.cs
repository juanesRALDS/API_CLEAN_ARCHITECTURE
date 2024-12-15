using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Users
{
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
            var user = new User { Id = userId, Name = "User1", Email = "user1@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);

            // Act
            var result = await _useCase.Execute(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("User1", result.Name);
            Assert.Equal("user1@example.com", result.Email);
        }

        [Fact]
        public async Task Execute_ShouldThrowArgumentNullException_WhenUserIdIsNull()
        {
            // Arrange
            string? userId = null;

            // Act
            Func<Task> act = async () => await _useCase.Execute(userId);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
            _userRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Execute_ShouldThrowArgumentException_WhenUserIdIsEmpty()
        {
            // Arrange
            var userId = string.Empty;

            // Act
            Func<Task> act = async () => await _useCase.Execute(userId);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
            _userRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Execute_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonExistentUserId";
            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _useCase.Execute(userId);

            // Assert
            Assert.Null(result);
        }
    }
}