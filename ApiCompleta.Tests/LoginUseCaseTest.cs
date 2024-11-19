using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
{
    public class LoginUserUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            string expectedToken = "valid_jwt_token";
            mockAuthService
                .Setup(auth => auth.LoginAsync(loginDto))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await useCase.ExecuteAsync(loginDto);

            // Assert
            mockAuthService.Verify(auth => auth.LoginAsync(loginDto), Times.Once);
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidCredentials_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var loginDto = new LoginUserDto
            {
                Email = "invalid@example.com",
                Password = "wrongpassword"
            };

            mockAuthService
                .Setup(auth => auth.LoginAsync(loginDto))
                .ThrowsAsync(new UnauthorizedAccessException("Credenciales inválidas."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => useCase.ExecuteAsync(loginDto));
            Assert.Equal("Credenciales inválidas.", exception.Message);
            mockAuthService.Verify(auth => auth.LoginAsync(loginDto), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyEmailOrPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var loginDto = new LoginUserDto
            {
                Email = "", // Empty email
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(loginDto));
            mockAuthService.Verify(auth => auth.LoginAsync(It.IsAny<LoginUserDto>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldCallAuthService()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var userDto = new CreateUserDto
            {
                Name = "Juan Perez",
                Email = "juan.perez@example.com",
                Password = "password123"
            };

            // Act
            await useCase.RegisterAsync(userDto);

            // Assert
            mockAuthService.Verify(auth => auth.RegisterAsync(userDto), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithMissingFields_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var userDto = new CreateUserDto
            {
                Name = "Juan Perez",
                Email = "", // Missing email
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.RegisterAsync(userDto));
            mockAuthService.Verify(auth => auth.RegisterAsync(It.IsAny<CreateUserDto>()), Times.Never);
        }
    }
}
