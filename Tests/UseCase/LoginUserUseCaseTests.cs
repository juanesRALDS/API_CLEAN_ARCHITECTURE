using Xunit;
using Moq;
using FluentAssertions;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities; // Asegúrate de que este namespace exista
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace api_completa_mongodb_net_6_0.Test.UseCases
{
    public class LoginUserUseCaseTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IOptions<JwtConfig>> _mockJwtConfig;
        private readonly LoginUserUseCase _loginUserUseCase;

        public LoginUserUseCaseTests()
        {
            // Inicializar Mocks
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockJwtConfig = new Mock<IOptions<JwtConfig>>();

            // Configurar una instancia simulada de JwtConfig
            var jwtConfig = new JwtConfig { Secret = "MiClaveSecreta12345" };
            _mockJwtConfig.Setup(x => x.Value).Returns(jwtConfig);

            // Crear la instancia de LoginUserUseCase con dependencias simuladas
            _loginUserUseCase = new LoginUserUseCase(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockJwtConfig.Object
            );
        }

        [Fact]
        public async void Login_ValidCredentials_ReturnsToken()
        {
            // Configurar los mocks para simular el comportamiento esperado
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(new User { Email = "test@example.com", PasswordHash = "hashed_password" });

            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword("hashed_password", "password123"))
                .Returns(true);

            // Actuar
            var result = await _loginUserUseCase.Execute("test@example.com", "password123");

            // Verificar los resultados
            result.Should().NotBeNullOrEmpty("el método debería devolver un token válido.");
            _mockUserRepository.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Once);
            _mockPasswordHasher.Verify(hasher => hasher.VerifyHashedPassword("hashed_password", "password123"), Times.Once);
        }

        [Fact]
        public async void Login_InvalidCredentials_ThrowsException()
        {
            // Configurar los mocks para simular credenciales inválidas
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync((User)null); // Usuario no encontrado

            // Actuar
            var act = async () => await _loginUserUseCase.Execute("invalid@example.com", "wrongpassword");

            // Verificar que se arroje una excepción
            await act.Should().ThrowAsync<UnauthorizedAccessException>("las credenciales son inválidas.");
        }
    }
}
