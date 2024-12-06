using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using Moq;
using Xunit;

namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
{
    public class LoginUserUseCaseTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
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
            var result = await useCase.Login(loginDto);

            // Assert
            mockAuthService.Verify(auth => auth.LoginAsync(loginDto), Times.Once);
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public async Task Login_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.Login(new()));
        }

        [Fact]
        public async Task Login_WithEmptyEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var loginDto = new LoginUserDto
            {
                Email = "",
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Login(loginDto));
            mockAuthService.Verify(auth => auth.LoginAsync(It.IsAny<LoginUserDto>()), Times.Never);
        }

        [Fact]
        public async Task Login_WithEmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Login(loginDto));
            mockAuthService.Verify(auth => auth.LoginAsync(It.IsAny<LoginUserDto>()), Times.Never);
        }

        [Fact]
        public async Task Register_WithValidData_ShouldCallAuthService()
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
            await useCase.Register(userDto);

            // Assert
            mockAuthService.Verify(auth => auth.Register(userDto), Times.Once);
        }

        [Fact]
        public async Task Register_WithMissingName_ShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            var userDto = new CreateUserDto
            {
                Name = "",
                Email = "juan.perez@example.com",
                Password = "password123"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Register(userDto));
            mockAuthService.Verify(auth => auth.Register(It.IsAny<CreateUserDto>()), Times.Never);
        }

        [Fact]
        public async Task LoginWithNullDtoShouldThrowArgumentException()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            var useCase = new LoginUserUseCase(mockAuthService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.Login(new LoginUserDto
            {
                Email = "",
                Password = "password123"
            }));
            // Act & Assert
            ArgumentException? exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.Login(null));
            Assert.Equal("Email cannot be empty or whitespace.", exception.Message);
        }




    }
}
