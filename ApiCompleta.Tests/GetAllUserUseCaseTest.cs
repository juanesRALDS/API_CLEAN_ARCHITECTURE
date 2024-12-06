using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
{
    public class GetAllUsersUseCaseTests
    {
        [Fact]
        public async Task LoginWithValidPaginationShouldReturnMappedUserList()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var useCase = new GetAllUsersUseCase(mockUserRepository.Object);

            int pageNumber = 1;
            int pageSize = 2;

            var users = new List<User>
            {
                new User { Id = "1", Name = "John Doe", Email = "john@example.com" },
                new User { Id = "2", Name = "Jane Doe", Email = "jane@example.com" }
            };

            mockUserRepository
                .Setup(repo => repo.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(users);

            // Act
            var result = await useCase.Login(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result[0].Id);
            Assert.Equal("John Doe", result[0].Name);
            Assert.Equal("john@example.com", result[0].Email);
            mockUserRepository.Verify(repo => repo.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task TestWithNoUsersShouldReturnEmptyList()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var useCase = new GetAllUsersUseCase(mockUserRepository.Object);

            int pageNumber = 1;
            int pageSize = 10;

            mockUserRepository
                .Setup(repo => repo.GetAllAsync(pageNumber, pageSize))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await useCase.Login(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            mockUserRepository.Verify(repo => repo.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task Login_WhenRepositoryThrowsException_ShouldThrowSameException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var useCase = new GetAllUsersUseCase(mockUserRepository.Object);

            int pageNumber = 1;
            int pageSize = 10;

            mockUserRepository
                .Setup(repo => repo.GetAllAsync(pageNumber, pageSize))
                .ThrowsAsync(new InvalidOperationException("Error en el repositorio"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.Login(pageNumber, pageSize));
            Assert.Equal("Error en el repositorio", exception.Message);
            mockUserRepository.Verify(repo => repo.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task Login_WithInvalidPagination_ShouldThrowArgumentException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var useCase = new GetAllUsersUseCase(mockUserRepository.Object);

            int invalidPageNumber = 0; // Invalid page number
            int invalidPageSize = -5; // Invalid page size

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Login(invalidPageNumber, invalidPageSize));
            mockUserRepository.Verify(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
