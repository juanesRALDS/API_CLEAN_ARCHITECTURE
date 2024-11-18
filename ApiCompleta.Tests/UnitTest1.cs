using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ApiCompleta.Tests.Application.UseCases
{
    public class CreateUserUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly CreateUserUseCase _createUserUseCase;

        public CreateUserUseCaseTests()
        {
            // Crear mocks para las interfaces
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            // Inicializar el caso de uso con los mocks
            _createUserUseCase = new CreateUserUseCase(_userRepositoryMock.Object, _passwordHasherMock.Object);
        }

        [Fact]
        public async Task Should_Create_User_Successfully()
        {
            // Arrange (Preparación)
            var createUserDto = new CreateUserDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "SuperSecret123"
            };

            var hashedPassword = "hashedPassword123";
            
            // Configurar el mock para que devuelva la contraseña hasheada
            _passwordHasherMock
                .Setup(ph => ph.HashPassword(createUserDto.Password))
                .Returns(hashedPassword);

            // Act (Ejecución)
            await _createUserUseCase.ExecuteAsync(createUserDto);

            // Assert (Verificación)
            _passwordHasherMock.Verify(ph => ph.HashPassword(createUserDto.Password), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<User>(u =>
                u.Name == createUserDto.Name &&
                u.Email == createUserDto.Email &&
                u.Password == hashedPassword
            )), Times.Once);
        }
    }
}
