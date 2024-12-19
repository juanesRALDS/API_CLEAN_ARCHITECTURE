using Xunit;
using Moq;
using FluentAssertions;
using SagaAserhi.Application.DTO.Auth;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Config;
using SagaAserhi.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using SagaAserhi.Application.UseCases.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.Utils;

namespace SagaAserhi.Test.UseCases;
public class LoginUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IOptions<JwtConfig>> _mockJwtConfig;
    private readonly ILoginUseCase _loginUserUseCase;

    public LoginUserUseCaseTests()
    {
        // Inicializar Mocks
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtConfig = new Mock<IOptions<JwtConfig>>();

        // Configurar una instancia simulada de JwtConfig
        JwtConfig? jwtConfig = new()
        {
            SecretKey = "clave_secreta_muy_segura_y_larga_12dkaslskl345",
            Issuer = "my_app",
            Audience = "my_app"
        };

        _mockJwtConfig.Setup(x => x.Value).Returns(jwtConfig);

        // Crear la instancia de LoginUserUseCase con dependencias simuladas
        _loginUserUseCase = new LoginUserUseCase(
            _mockUserRepository.Object,
            _mockJwtConfig.Object,
            _mockPasswordHasher.Object
        );
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Configurar los mocks para simular el comportamiento esperado
        LoginUserDto? loginDto = new() { Email = "test@example.com", Password = "password123" };

        // Configurar mocks
        _mockUserRepository.Setup(repo => repo.GetUserByEmail(loginDto.Email))
            .ReturnsAsync(new User
            {
                Id = "507f1f77bcf86cd799439011",
                Email = loginDto.Email,
                Password = "hashed_password",
                Name = "Test User"
            });

        _mockPasswordHasher.Setup(hasher => hasher.VerifyPassword(loginDto.Password, "hashed_password"))
            .Returns(true);

        // Actuar
        string? result = await _loginUserUseCase.Execute(loginDto);

        // Verificar los resultados
        result.Should().NotBeNullOrEmpty("el método debería devolver un token válido.");
        _mockUserRepository.Verify(repo => repo.GetUserByEmail(loginDto.Email), Times.Once);
        _mockPasswordHasher.Verify(hasher => hasher.VerifyPassword(loginDto.Password, "hashed_password"), Times.Once);
    }

    [Fact]
    public async void Login_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Configurar los mocks para simular credenciales inválidas
        LoginUserDto? loginDto = new() { Email = "invalid@example.com", Password = "wrongpassword" };

        _mockUserRepository.Setup(repo => repo.GetUserByEmail(loginDto.Email))
            .ReturnsAsync((User?)null); // Usuario no encontrado

        // Actuar
        Func<Task> act = async () => await _loginUserUseCase.Execute(loginDto);

        // Verificar que se arroje una excepción
        await act.Should().ThrowAsync<UnauthorizedAccessException>("las credenciales son inválidas.");
    }

    [Fact]
    public async void Login_EmptyEmail_ThrowsArgumentException()
    {
        // Configurar el DTO con un email vacío
        LoginUserDto? loginDto = new() { Email = "", Password = "password123" };

        // Actuar
        Func<Task> act = async () => await _loginUserUseCase.Execute(loginDto);

        // Verificar que se arroje una excepción
        await act.Should().ThrowAsync<ArgumentException>("El email no puede estar vacío.");
    }

    [Fact]
    public async void Login_EmptyPassword_ThrowsArgumentException()
    {
        // Configurar el DTO con una contraseña vacía
        LoginUserDto? loginDto = new() { Email = "test@example.com", Password = "" };

        // Actuar
        Func<Task> act = async () => await _loginUserUseCase.Execute(loginDto);

        // Verificar que se arroje una excepción
        await act.Should().ThrowAsync<ArgumentException>("La contraseña no puede estar vacía.");
    }
}
