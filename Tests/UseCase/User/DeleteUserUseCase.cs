using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Users
{
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
        public async Task DeleteUser_ShouldDeleteUser_WhenIdIsValid()
        {
            // Arrange
            var userId = "validUserId";
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _useCase.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowArgumentNullException_WhenIdIsNull()
        {
            // Arrange
            string? userId = null;

            // Act
            Func<Task> act = async () => await _useCase.DeleteUser(userId);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Arrange
            var userId = string.Empty;

            // Act
            Func<Task> act = async () => await _useCase.DeleteUser(userId);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
        }
    }
}