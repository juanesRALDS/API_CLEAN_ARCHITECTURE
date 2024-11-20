// using System.Threading.Tasks;
// using api_completa_mongodb_net_6_0.Application.DTO;
// using api_completa_mongodb_net_6_0.Application.UseCases;
// using api_completa_mongodb_net_6_0.Domain.Entities;
// using api_completa_mongodb_net_6_0.Domain.Interfaces;
// using Moq;
// using Xunit;

// namespace api_completa_mongodb_net_6_0.ApiCompleta.Tests
// {
//     public class UpdateUserUseCaseTests
//     {
//         [Fact]
//         public async Task ExecuteAsync_WithValidData_ShouldCallRepositoryAndReturnUpdatedUser()
//         {
//             // Arrange
//             var mockUserRepository = new Mock<IUserRepository>();
//             var mockPasswordHasher = new Mock<IPasswordHasher>();

//             var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

//             var dto = new UpdateUserDto
//             {
//                 Name = "Juan Jose",
//                 Email = "juanjose@example.com",
//                 Password = "newpassword123"
//             };

//             string hashedPassword = "hashed_password";
//             mockPasswordHasher
//                 .Setup(hasher => hasher.HashPassword(dto.Password))
//                 .Returns(hashedPassword);

//             var existingUser = new User
//             {
//                 Id = "1",
//                 Name = "Old Name",
//                 Email = "oldemail@example.com",
//                 Password = "oldpassword"
//             };

//             mockUserRepository
//                 .Setup(repo => repo.GetByIdAsync("1"))
//                 .ReturnsAsync(existingUser);

//             var expectedResponse = new UpdateUserResponseDto
//             {
//                 Id = "1",
//                 Name = dto.Name,
//                 Email = dto.Email
//             };

//             // Act
//             var result = await useCase.ExecuteAsync("1", dto);

//             // Assert
//             mockPasswordHasher.Verify(hasher => hasher.HashPassword(dto.Password), Times.Once);
//             mockUserRepository.Verify(repo => repo.UpdateAsync("1", It.Is<User>(user =>
//                 user.Name == dto.Name &&
//                 user.Email == dto.Email &&
//                 user.Password == hashedPassword
//             )), Times.Once);

//             Assert.Equal(expectedResponse.Id, result.Id);
//             Assert.Equal(expectedResponse.Name, result.Name);
//             Assert.Equal(expectedResponse.Email, result.Email);
//         }

//         [Fact]
//         public async Task ExecuteAsync_WithMissingFields_ShouldThrowArgumentException()
//         {
//             // Arrange
//             var mockUserRepository = new Mock<IUserRepository>();
//             var mockPasswordHasher = new Mock<IPasswordHasher>();

//             var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

//             var dto = new UpdateUserDto
//             {
//                 Name = "", // Missing
//                 Email = "juanjose@example.com",
//                 Password = "newpassword123"
//             };

//             var existingUser = new User
//             {
//                 Id = "1",
//                 Name = "Juan Jose",
//                 Email = "juanjose@example.com",
//                 Password = "oldpassword"
//             };

//             mockUserRepository
//                 .Setup(repo => repo.GetByIdAsync("1"))
//                 .ReturnsAsync(existingUser);

//             // Act & Assert
//             await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync("1", dto));

//             mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
//             mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
//         }

//         [Fact]
//         public async Task ExecuteAsync_WithNonExistentUserId_ShouldThrowKeyNotFoundException()
//         {
//             // Arrange
//             var mockUserRepository = new Mock<IUserRepository>();
//             var mockPasswordHasher = new Mock<IPasswordHasher>();

//             var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

//             var dto = new UpdateUserDto
//             {
//                 Name = "Juan Jose",
//                 Email = "juanjose@example.com",
//                 Password = "newpassword123"
//             };

//             // Simulating a non-existent user
//             mockUserRepository
//                 .Setup(repo => repo.GetByIdAsync("1"))
//                 .ReturnsAsync((User)null);

//             // Act & Assert
//             await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync("1", dto));

//             mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
//             mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
//         }

//         [Fact]
//         public async Task ExecuteAsync_WithInvalidEmailFormat_ShouldThrowFormatException()
//         {
//             // Arrange
//             var mockUserRepository = new Mock<IUserRepository>();
//             var mockPasswordHasher = new Mock<IPasswordHasher>();

//             var useCase = new UpdateUserUseCase(mockUserRepository.Object, mockPasswordHasher.Object);

//             var dto = new UpdateUserDto
//             {
//                 Name = "Juan Jose",
//                 Email = "invalid-email", // Invalid email
//                 Password = "newpassword123"
//             };

//             var existingUser = new User
//             {
//                 Id = "1",
//                 Name = "Juan Jose",
//                 Email = "juanjose@example.com",
//                 Password = "oldpassword"
//             };

//             mockUserRepository
//                 .Setup(repo => repo.GetByIdAsync("1"))
//                 .ReturnsAsync(existingUser);

//             // Act & Assert
//             await Assert.ThrowsAsync<FormatException>(() => useCase.ExecuteAsync("1", dto));

//             mockPasswordHasher.Verify(hasher => hasher.HashPassword(It.IsAny<string>()), Times.Never);
//             mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()), Times.Never);
//         }
//     }
// }
