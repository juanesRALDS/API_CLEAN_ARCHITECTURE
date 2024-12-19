using SagaAserhi.Application.DTO.Auth;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.Utils;

namespace SagaAserhi.Tests.Application.UseCases.Auth;

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
        CreateUserDto? userDto = new()
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
        string? result = await _useCase.Execute(userDto);

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
        CreateUserDto? userDto = new()
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
        CreateUserDto? userDto = new()
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
        CreateUserDto? userDto = new()
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
        CreateUserDto? userDto = new()
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
        CreateUserDto? userDto = new()
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