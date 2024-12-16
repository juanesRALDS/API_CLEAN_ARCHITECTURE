using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Auth;

public class RegisterUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly IOptions<JwtConfig> _jwtConfig;
    private readonly IRegisterUseCase _useCase;

    public RegisterUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();

        _jwtConfig = Options.Create(new JwtConfig
        {
            SecretKey = "SuperSecretKeyForTestinlsadklaslkdsallg",
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        });

        _useCase = new RegisterUseCase(
            _userRepositoryMock.Object,
            _jwtConfig,
            _passwordHasherMock.Object
        );
    }

    [Fact]
    public async Task ShouldRegisterUserSuccessfully()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(userDto.Email))
            .ReturnsAsync((User?)null);

        _passwordHasherMock.Setup(hasher => hasher.HashPassword(userDto.Password))
            .Returns("hashedPassword");

        _userRepositoryMock.Setup(repo => repo.CreateNewUser(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(userDto);

        // Assert
        result.Should().Be("User registered successfully");

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(userDto.Email), Times.Once);
        _passwordHasherMock.Verify(hasher => hasher.HashPassword(userDto.Password), Times.Once);
        _userRepositoryMock.Verify(repo => repo.CreateNewUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task WhenUserDtoIsNull()
    {
        // Act
        CreateUserDto? nullDto = null;
        Func<Task> act = async () => await _useCase.Execute(nullDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'userDto')");
    }

    [Fact]
    public async Task WhenNameIsEmpty()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "",
            Email = "test@example.com",
            Password = "Password123"
        };

        // Act
        Func<Task> act = async () => await _useCase.Execute(userDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("the name cannot be empty or null (Parameter 'Name')");
    }

    [Fact]
    public async Task WhenEmailIsEmpty()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "",
            Password = "Password123"
        };

        // Act
        Func<Task> act = async () => await _useCase.Execute(userDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Email (parameter 'the email cannot be empty or null')");
    }

    [Fact]
    public async Task WhenPasswordIsEmpty()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = ""
        };

        // Act
        Func<Task> act = async () => await _useCase.Execute(userDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("the password cannot be empty or null (Parameter 'Password')");
    }

    [Fact]
    public async Task WhenEmailAlreadyExists()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Password123"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(userDto.Email))
            .ReturnsAsync(new User());

        // Act
        Func<Task> act = async () => await _useCase.Execute(userDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A user with this email already exists.");

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(userDto.Email), Times.Once);
    }

    [Fact]
    public async Task WhenEmailIsInvalid()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "invalid-email",
            Password = "Password123"
        };

        // Act
        Func<Task> act = async () => await _useCase.Execute(userDto);

        // Assert
        await act.Should().ThrowAsync<FormatException>()
            .WithMessage("The email format is invalid.");
    }
}