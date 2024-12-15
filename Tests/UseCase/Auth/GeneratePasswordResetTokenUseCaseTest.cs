// using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
// using api_completa_mongodb_net_6_0.Domain.Entities;
// using api_completa_mongodb_net_6_0.Domain.Interfaces;
// using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
// using api_completa_mongodb_net_6_0.Infrastructure.Config;
// using FluentAssertions;
// using Microsoft.Extensions.Options;
// using Moq;
// using System;
// using System.Threading.Tasks;
// using Xunit;

// namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Auth
// {
//     public class GeneratePasswordResetTokenUseCaseTests
//     {
//         private readonly Mock<IUserRepository> _userRepositoryMock;
//         private readonly Mock<IPasswordResetTokenRepository> _tokenRepositoryMock;
//         private readonly Mock<IEmailService> _emailServiceMock;
//         private readonly IOptions<JwtConfig> _jwtConfig;
//         private readonly GeneratePasswordResetTokenUseCase _useCase;

//         public GeneratePasswordResetTokenUseCaseTests()
//         {
//             _userRepositoryMock = new Mock<IUserRepository>();
//             _tokenRepositoryMock = new Mock<IPasswordResetTokenRepository>();
//             _emailServiceMock = new Mock<IEmailService>();

//             _jwtConfig = Options.Create(new JwtConfig
//             {
//                 SecretKey = "SuperSecretKeyForTestingdfdsfdsdsf",
//                 Issuer = "TestIssuer",
//                 Audience = "TestAudience"
//             });

//             _useCase = new GeneratePasswordResetTokenUseCase(
//                 _userRepositoryMock.Object,
//                 _tokenRepositoryMock.Object,
//                 _jwtConfig.Value,
//                 _emailServiceMock.Object,
//                 _jwtConfig
//             );
//         }

//         [Fact]
//         public async Task Execute_ShouldGenerateTokenAndSendEmail_WhenUserExists()
//         {
//             // Arrange
//             var email = "test@example.com";
//             var user = new User { Id = "123", Email = email, Name = "Test User" };

//             _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
//                 .ReturnsAsync(user);

//             _emailServiceMock.Setup(service => service.SendEmailAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()
//             )).Returns(Task.CompletedTask);

//             _tokenRepositoryMock.Setup(repo => repo.SaveToken(It.IsAny<Token>()))
//                 .Returns(Task.CompletedTask);

//             // Act
//             var result = await _useCase.Execute(email);

//             // Assert
//             result.Should().Be("El enlace para restablecer la contraseña ha sido enviado a tu correo electrónico.");

//             _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
//             _emailServiceMock.Verify(service => service.SendEmailAsync(
//                 email,
//                 It.Is<string>(s => s.Contains("Restablecimiento de contraseña")),
//                 It.Is<string>(b => b.Contains("Solicitud de Restablecimiento de Contraseña"))
//             ), Times.Once);
//             _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Once);
//         }

//         [Fact]
//         public async Task Execute_ShouldThrowException_WhenUserDoesNotExist()
//         {
//             // Arrange
//             var email = "nonexistent@example.com";

//             _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
//                 .ReturnsAsync((User?)null);

//             // Act
//             Func<Task> act = async () => await _useCase.Execute(email);

//             // Assert
//             await act.Should().ThrowAsync<Exception>()
//                 .WithMessage("Usuario no encontrado con el correo proporcionado.");

//             _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
//             _emailServiceMock.Verify(service => service.SendEmailAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()
//             ), Times.Never);
//             _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Never);
//         }

//         [Fact]
//         public async Task Execute_ShouldThrowInvalidOperationException_WhenEmailSendingFails()
//         {
//             // Arrange
//             var email = "test@example.com";
//             var user = new User { Id = "123", Email = email, Name = "Test User" };

//             _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
//                 .ReturnsAsync(user);

//             _emailServiceMock.Setup(service => service.SendEmailAsync(
//                 It.IsAny<string>(),
//                 It.IsAny<string>(),
//                 It.IsAny<string>()
//             )).ThrowsAsync(new InvalidOperationException("Error de prueba al enviar correo."));

//             // Act
//             Func<Task> act = async () => await _useCase.Execute(email);

//             // Assert
//             await act.Should().ThrowAsync<InvalidOperationException>()
//                 .WithMessage("Error al enviar el correo Error de prueba al enviar correo.");

//             _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
//             _emailServiceMock.Verify(service => service.SendEmailAsync(
//                 email,
//                 It.Is<string>(s => s.Contains("Restablecimiento de contraseña")),
//                 It.Is<string>(b => b.Contains("Solicitud de Restablecimiento de Contraseña"))
//             ), Times.Once);
//             _tokenRepositoryMock.Verify(repo => repo.SaveToken(It.IsAny<Token>()), Times.Never);
//         }
//     }
// }
