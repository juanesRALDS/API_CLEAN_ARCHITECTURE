using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase
{
    public class CreatePotentialClientUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly CreatePotentialClientUseCase _useCase;
        private readonly CreatePotentialClientDto _validDto;

        public CreatePotentialClientUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new CreatePotentialClientUseCase(_mockRepository.Object);
            _validDto = new CreatePotentialClientDto
            {
                Identification = new Identification 
                { 
                    Type = "NIT",
                    Number = "123456789" 
                },
                BusinessInfo = new BusinessInfo
                {
                    TradeName = "Empresa Prueba",
                    EconomicActivity = "Desarrollo Software",
                    Email = "test@empresa.com",
                    Phone = "3001234567"
                },
                Status = "Activo"
            };
        }

        [Fact]
        public async Task Execute_ConDatosNulos_DebeLanzarArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _useCase.Execute(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Execute_ConTipoIdentificacionInvalida_DebeLanzarArgumentException(string tipoInvalido)
        {
            // Arrange
            CreatePotentialClientDto? dtoConTipoInvalido = new CreatePotentialClientDto
            {
                Identification = new Identification { Type = tipoInvalido, Number = _validDto.Identification.Number },
                BusinessInfo = _validDto.BusinessInfo,
                Status = _validDto.Status
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(dtoConTipoInvalido));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Execute_ConNumeroIdentificacionInvalido_DebeLanzarArgumentException(string numeroInvalido)
        {
            // Arrange
             CreatePotentialClientDto? dtoConNumeroInvalido = new CreatePotentialClientDto
            {
                Identification = new Identification { Type = _validDto.Identification.Type, Number = numeroInvalido },
                BusinessInfo = _validDto.BusinessInfo,
                Status = _validDto.Status
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(dtoConNumeroInvalido));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("correo-invalido")]
        public async Task Execute_ConEmailInvalido_DebeLanzarArgumentException(string emailInvalido)
        {
            // Arrange
            var dtoConEmailInvalido = new CreatePotentialClientDto
            {
                Identification = _validDto.Identification,
                BusinessInfo = new BusinessInfo { 
                    TradeName = _validDto.BusinessInfo.TradeName,
                    EconomicActivity = _validDto.BusinessInfo.EconomicActivity,
                    Email = emailInvalido,
                    Phone = _validDto.BusinessInfo.Phone
                },
                Status = _validDto.Status
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(dtoConEmailInvalido));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Estado Invalido")]
        public async Task Execute_ConEstadoInvalido_DebeLanzarArgumentException(string estadoInvalido)
        {
            // Arrange
            var dtoConEstadoInvalido = new CreatePotentialClientDto
            {
                Identification = _validDto.Identification,
                BusinessInfo = _validDto.BusinessInfo,
                Status = estadoInvalido
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(dtoConEstadoInvalido));
        }

        [Fact]
        public async Task Execute_ConDatosValidos_DebeRetornarMensajeExito()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PotentialClient>());

            // Act
            var resultado = await _useCase.Execute(_validDto);

            // Assert
            Assert.Equal("Cliente potencial creado exitosamente", resultado);
            _mockRepository.Verify(r => r.CreatePotentialClient(It.IsAny<PotentialClient>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ConIdentificacionDuplicada_DebeLanzarInvalidOperationException()
        {
            // Arrange
            var clientesExistentes = new List<PotentialClient>
            {
                new() { 
                    Identification = new Identification { 
                        Number = "123456789" 
                    } 
                }
            };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientesExistentes);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(_validDto));
        }

        [Fact]
        public async Task Execute_ConEmailDuplicado_DebeLanzarInvalidOperationException()
        {
            // Arrange
            var clientesExistentes = new List<PotentialClient>
            {
                new() { 
                    BusinessInfo = new BusinessInfo { 
                        Email = "test@empresa.com" 
                    } 
                }
            };

            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientesExistentes);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(_validDto));
        }
    }
}