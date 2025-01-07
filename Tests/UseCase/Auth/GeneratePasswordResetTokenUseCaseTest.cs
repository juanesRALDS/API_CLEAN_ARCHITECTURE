using SagaAserhi.Application.Interfaces.Auth;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.Application.UseCases.Auth;

public class GeneratePasswordResetTokenUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordResetTokenRepository> _tokenRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IGeneratePasswordResetTokenUseCase _useCase;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public GeneratePasswordResetTokenUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenRepositoryMock = new Mock<IPasswordResetTokenRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        DefaultHttpContext? httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        _jwtConfig = Options.Create(new JwtConfig
        {
            SecretKey = "clave_secreta_de_prueba",
            ExpirationMinutes = 60
        });

        _jwtConfig = Options.Create(new JwtConfig
        {
            SecretKey = "SuperSecretKeyForTestingdfdsfdsdsf",
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        });

        _useCase = new GeneratePasswordResetTokenUseCase(
            _userRepositoryMock.Object,
            _tokenRepositoryMock.Object,
            _emailServiceMock.Object,      // Cambiado de _jwtConfig.Value
            _jwtConfig,                    // Cambiado de _emailServiceMock.Object
            _httpContextAccessorMock.Object // Nuevo - necesitas crear este mock
        );
    }

    [Fact]
    public async Task Execute_ShouldGenerateTokenAndSendEmail_WhenUserExists()
    {
        // Arrange
        string? email = "test@example.com";
        User? user = new() { Id = "123", Email = email, Name = "Test User" };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
            .ReturnsAsync(user);

        _emailServiceMock.Setup(service => service.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        )).Returns(Task.CompletedTask);

        _tokenRepositoryMock.Setup(repo => repo.SaveToken(It.IsAny<Token>()))
            .Returns(Task.CompletedTask);

        // Act
        string? result = await _useCase.Execute(email);

        // Assert
        result.Should().Be("El enlace para restablecer la contraseña ha sido enviado a tu correo electrónico.");

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailAsync(
            email,
            It.Is<string>(s => s.Contains("Restablecimiento de contraseña")),
            It.Is<string>(b => b.Contains("Solicitud de Restablecimiento de Contraseña"))
        ), Times.Once);
        _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        string? email = "nonexistent@example.com";

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = async () => await _useCase.Execute(email);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Usuario no encontrado con el correo proporcionado.");

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        ), Times.Never);
        _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenEmailSendingFails()
    {
        // Arrange
        string? email = "test@example.com";
        User? user = new() { Id = "123", Email = email, Name = "Test User" };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
            .ReturnsAsync(user);

        _emailServiceMock.Setup(service => service.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()
        )).ThrowsAsync(new Exception("Error de prueba al enviar correo."));

         // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>( async () => await _useCase.Execute(email));

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailAsync(
            email,
            It.Is<string>(s => s.Contains("Restablecimiento de contraseña")),
            It.Is<string>(b => b.Contains("Solicitud de Restablecimiento de Contraseña"))
        ), Times.Once);
        _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Never);
    }
}
