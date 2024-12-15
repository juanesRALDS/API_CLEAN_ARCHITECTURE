using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace api_completa_mongodb_net_6_0.Tests.Application.UseCases.Auth
{
    public class RegisterUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly IOptions<JwtConfig> _jwtConfig;
        private readonly RegisterUseCase _useCase;

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
        public async Task Execute_ShouldRegisterUserSuccessfully()
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
        public async Task Execute_ShouldThrowArgumentNullException_WhenUserDtoIsNull()
        {
            // Act
            Func<Task> act = async () => await _useCase.Execute(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'userDto')");
        }

        [Fact]
        public async Task Execute_ShouldThrowArgumentException_WhenNameIsEmpty()
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
                .WithMessage("El nombre no puede estar vacío o nulo. (Parameter 'userDto.Name')");
        }

        [Fact]
        public async Task Execute_ShouldThrowArgumentNullException_WhenEmailIsEmpty()
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
                .WithMessage("el correo no puede ser nulo (Parameter 'userDto.Email')");
        }

        [Fact]
        public async Task Execute_ShouldThrowArgumentException_WhenPasswordIsEmpty()
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
                .WithMessage("la contraseña no puede  estar vacia o ser nula  (Parameter 'userDto.Password')");
        }

        [Fact]
        public async Task Execute_ShouldThrowInvalidOperationException_WhenEmailAlreadyExists()
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
                .WithMessage("Ya existe un usuario registrado con este correo.");

            _userRepositoryMock.Verify(repo => repo.GetUserByEmail(userDto.Email), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldThrowFormatException_WhenEmailIsInvalid()
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
                .WithMessage("El correo electrónico no tiene un formato válido.");
        }
    }
}