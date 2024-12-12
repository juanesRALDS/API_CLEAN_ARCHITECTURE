// using System;
// using System.Threading.Tasks;
// using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
// using api_completa_mongodb_net_6_0.Domain.Entities;
// using api_completa_mongodb_net_6_0.Domain.Interfaces;
// using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
// using Microsoft.IdentityModel.Tokens;
// using MongoApiDemo.Domain.Interfaces.Utils;
// using Moq;
// using Xunit;

// namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Auth;
// public class UpdatePasswordUseCaseTests
// {
//     private readonly Mock<IPasswordResetTokenRepository> _mockTokenRepository;
//     private readonly Mock<IUserRepository> _mockUserRepository;
//     private readonly UpdatePasswordUseCase _useCase;
//     private readonly Mock<IPasswordHasher> _mockPasswordHelper;

//     public UpdatePasswordUseCaseTests()
//     {
//         _mockTokenRepository = new Mock<IPasswordResetTokenRepository>();
//         _mockUserRepository = new Mock<IUserRepository>();
//         _mockPasswordHelper = new Mock<IPasswordHasher>();
//         _useCase = new UpdatePasswordUseCase(_mockTokenRepository.Object, _mockUserRepository.Object, _mockPasswordHelper.Object);
//     }

//     [Fact]
//     public async Task Execute_ShouldUpdatePassword_WhenTokenIsValid()
//     {

//         var mockTokenRepository = new Mock<IPasswordResetTokenRepository>();
//         var mockUserRepository = new Mock<IUserRepository>();
//         var mockPasswordHasher = new Mock<IPasswordHasher>();

//         // Arrange
//         string token = "valid-token";
//         string newPassword = "NewPassword123";
//         string hashedPassword = "hashed-password";

//         mockTokenRepository
//     .Setup(repo => repo.GetByTokenAsync(token))
//     .ReturnsAsync(new Token
//     {
//         Tokens = token,
//         Expiration = DateTime.UtcNow.AddMinutes(30),
//         UserId = "userId"
//     });

//         mockUserRepository
//             .Setup(repo => repo.GetByIdAsync("userId"))
//             .ReturnsAsync(new User { Id = "userId" });

//         mockPasswordHasher
//             .Setup(hasher => hasher.HashPassword(newPassword))
//             .Returns(hashedPassword);

//         var useCase = new UpdatePasswordUseCase(mockTokenRepository.Object, mockUserRepository.Object, mockPasswordHasher.Object);

//         // Act
//         var result = await useCase.Execute(token, newPassword);

//         // Assert
//         Assert.True(result);
//         mockUserRepository.Verify(repo => repo.UpdatePasswordAsync("userId", hashedPassword), Times.Once);
//         mockTokenRepository.Verify(repo => repo.DeleteTokenAsync(token), Times.Once);
//     }

//     [Fact]
//     public async Task Execute_ShouldThrow_WhenTokenNotFound()
//     {
//         // Arrange
//         string token = "invalid-token";
//         _mockTokenRepository.Setup(repo => repo.GetByTokenAsync(token))
//             .ReturnsAsync((Token?)null);

//         // Act & Assert
//         await Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             await _useCase.Execute(token, "NewPassword123"));
//     }

//     [Fact]
//     public async Task Execute_ShouldThrow_WhenTokenIsExpired()
//     {
//         // Arrange
//         string token = "expired-token";

//         var storedToken = new Token
//         {
//             Tokens = token,
//             Expiration = DateTime.UtcNow.AddMinutes(-10),
//             UserId = "user-id"
//         };

//         _mockTokenRepository.Setup(repo => repo.GetByTokenAsync(token))
//             .ReturnsAsync(storedToken);

//         // Act & Assert
//         await Assert.ThrowsAsync<SecurityTokenExpiredException>(async () =>
//             await _useCase.Execute(token, "NewPassword123"));
//     }

//     [Fact]
//     public async Task Execute_ShouldThrow_WhenUserNotFound()
//     {
//         // Arrange
//         string token = "valid-token";

//         var storedToken = new Token
//         {
//             Tokens = token,
//             Expiration = DateTime.UtcNow.AddMinutes(10),
//             UserId = "nonexistent-user-id"
//         };

//         _mockTokenRepository.Setup(repo => repo.GetByTokenAsync(token))
//             .ReturnsAsync(storedToken);

//         _mockUserRepository.Setup(repo => repo.GetByIdAsync(storedToken.UserId))
//             .ReturnsAsync((User?)null);

//         // Act & Assert
//         await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
//             await _useCase.Execute(token, "NewPassword123"));
//     }
// }
