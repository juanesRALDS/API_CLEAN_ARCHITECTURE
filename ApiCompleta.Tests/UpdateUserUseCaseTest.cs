using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
{
    public class UpdateUserUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldUpdateUserAndReturnResponse()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new UpdateUserDto
            {
                Name = "Juan Perez",
                Email = "juan.perez@example.com",
                Password = "securepassword123"
            };

            var hashedPassword = "hashed_password";
            mockPasswordHasher
                .Setup(h => h.HashPassword(dto.Password))
                .Returns(hashedPassword);

            var existingUser = new User
            {
                Id = "1",
                Name = "Old Name",
                Email = "old@example.com",
                Password = "oldpassword"
            };

            mockUserRepository
                .Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync(existingUser);

            // Act
            var result = await useCase.ExecuteAsync("1", dto);

            // Assert
            Assert.Equal("1", result.Id);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Email, result.Email);

            mockPasswordHasher.Verify(h => h.HashPassword(dto.Password), Times.Once);
            mockUserRepository.Verify(repo => repo.UpdateAsync("1", It.Is<User>(u =>
                u.Name == dto.Name &&
                u.Email == dto.Email &&
                u.Password == hashedPassword
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidEmail_ShouldThrowFormatException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new UpdateUserDto
            {
                Name = "Juan Perez",
                Email = "invalidemail",
                Password = "securepassword123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => useCase.ExecuteAsync("1", dto));
            mockPasswordHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithMissingFields_ShouldThrowArgumentException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new UpdateUserDto
            {
                Name = "Juan Perez",
                Email = "",
                Password = "securepassword123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync("1", dto));
            mockPasswordHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNonExistentUser_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordHasher = new Mock<IPasswordHasher>();
            var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

            var dto = new UpdateUserDto
            {
                Name = "Juan Perez",
                Email = "juan.perez@example.com",
                Password = "securepassword123"
            };

            mockUserRepository
                .Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync("1", dto));
            mockPasswordHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
        }

        
    }
}
