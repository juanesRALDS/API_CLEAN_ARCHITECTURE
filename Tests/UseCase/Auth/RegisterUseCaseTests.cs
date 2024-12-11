using System;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

public class RegisterUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly RegisterUseCase _registerUseCase;

    public RegisterUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();

        var jwtConfigMock = new Mock<IOptions<JwtConfig>>();
        jwtConfigMock.Setup(c => c.Value).Returns(new JwtConfig { SecretKey = "TestSecret" });
        _jwtConfig = jwtConfigMock.Object;

        _registerUseCase = new RegisterUseCase(_userRepositoryMock.Object, _jwtConfig, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentNullException_WhenUserDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _registerUseCase.Execute(null));
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "", Email = "test@example.com", Password = "password123" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _registerUseCase.Execute(userDto));
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentNullException_WhenEmailIsNull()
    {
        // Arrange
         CreateUserDto? userDto = new CreateUserDto { Name = "John", Email = null, Password = "password123" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _registerUseCase.Execute(userDto));
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentException_WhenPasswordIsEmpty()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "John", Email = "test@example.com", Password = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _registerUseCase.Execute(userDto));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenUserAlreadyExists()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "John", Email = "test@example.com", Password = "password123" };
        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(userDto.Email)).ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _registerUseCase.Execute(userDto));
    }

    [Fact]
    public async Task Execute_ShouldThrowFormatException_WhenEmailIsInvalid()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "John", Email = "invalid-email", Password = "password123" };

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => _registerUseCase.Execute(userDto));
    }

    [Fact]
    public async Task Execute_ShouldCreateUser_WhenDataIsValid()
    {
        // Arrange
        var userDto = new CreateUserDto { Name = "John", Email = "test@example.com", Password = "password123" };
        var hashedPassword = "hashedPassword123";

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(userDto.Email)).ReturnsAsync((User?)null);
        _passwordHasherMock.Setup(hasher => hasher.HashPassword(userDto.Password)).Returns(hashedPassword);

        // Act
        var result = await _registerUseCase.Execute(userDto);

        // Assert
        Assert.Equal("User registered successfully", result);
        _userRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<User>(u => u.Name == userDto.Name && u.Email == userDto.Email && u.Password == hashedPassword)), Times.Once);
    }
}
