using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.UseCase.Users
{
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
        public async Task Login_ShouldReturnUsers_WhenUsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Name = "User1", Email = "user1@example.com" },
                new User { Id = "2", Name = "User2", Email = "user2@example.com" }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(users);

            // Act
            var result = await _useCase.Execute(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("User1", result[0].Name);
            Assert.Equal("user1@example.com", result[0].Email);
            Assert.Equal("User2", result[1].Name);
            Assert.Equal("user2@example.com", result[1].Email);
        }

        [Fact]
        public async Task Login_ShouldThrowArgumentException_WhenPageNumberIsLessThanOrEqualToZero()
        {
            // Act
            Func<Task> act = async () => await _useCase.Execute(0, 10);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task Login_ShouldThrowArgumentException_WhenPageSizeIsLessThanOrEqualToZero()
        {
            // Act
            Func<Task> act = async () => await _useCase.Execute(1, 0);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }

        [Fact]
        public async Task Login_ShouldThrowInvalidOperationException_WhenRepositoryReturnsNull()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((List<User>)null);

            // Act
            Func<Task> act = async () => await _useCase.Execute(1, 10);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(act);
        }
    }
}