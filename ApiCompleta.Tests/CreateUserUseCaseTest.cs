using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
{
    public class CreateUserUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldCallRepositoryAndHasher()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();

            var useCase = new CreateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new CreateUserDto
            {
                Name = "juan jose",
                Email = "juanjose@example.com",
                Password = "password123"
            };

            string hashedPassword = "hashed_password";
            mockPasswordHasher
                .Setup(hasher => hasher.HashPassword(dto.Password))
                .Returns(hashedPassword);

            // Act
            await useCase.ExecuteAsync(dto);

            // Assert
            mockPasswordHasher.Verify(hasher => hasher.HashPassword(dto.Password), Times.Once);
            mockUserRepository.Verify(repo => repo.CreateAsync(It.Is<User>(user =>
                user.Name == dto.Name &&
                user.Email == dto.Email &&
                user.Password == hashedPassword
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithMissingName_ShouldThrowException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();

            var useCase = new CreateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new CreateUserDto
            {
                Name = null, // Faltante
                Email = "juanjose@example.com",
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.ExecuteAsync(dto));

            mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyPassword_ShouldThrowException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();

            var useCase = new CreateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new CreateUserDto
            {
                Name = "juan jose",
                Email = "juanjose@example.com",
                Password = string.Empty 
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));

            mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();

            var useCase = new CreateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new CreateUserDto
            {
                Name = "juan jose",
                Email = "not-an-email", 
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => useCase.ExecuteAsync(dto));

            mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
