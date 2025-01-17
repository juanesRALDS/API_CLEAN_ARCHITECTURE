using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.DTO.PotentialClientDto;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase
{
    public class UpdatePotentialClientUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly UpdatePotentialClientUseCase _useCase;
        private readonly DateTime _testDate;
        
        private const string VALID_ID = "valid_id";
        private const string COMPANY_NAME = "Test Company";
        private const string PHONE = "123456789";
        private const string EMAIL = "test@company.com";
        private const string STATUS = "Activo";

        public UpdatePotentialClientUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new UpdatePotentialClientUseCase(_mockRepository.Object);
            _testDate = DateTime.UtcNow;
        }

        private PotentialClient CreateExistingClient()
        {
            return new PotentialClient
            {
                Id = VALID_ID,
                BusinessInfo = new BusinessInfo
                {
                    TradeName = COMPANY_NAME,
                    Email = EMAIL,
                    Phone = PHONE
                },
                Status = new Status
                {
                    Current = STATUS,
                    History = new List<StatusHistory>
                    {
                        new()
                        {
                            Status = STATUS,
                            Date = _testDate,
                            Observation = "Estado inicial"
                        }
                    }
                },
                CreatedAt = _testDate,
                UpdatedAt = _testDate
            };
        }

        [Fact]
        public async Task Execute_ConDatosValidos_DebeActualizarCliente()
        {
            // Arrange
            var existingClient = CreateExistingClient();
            var updateDto = new UpdatePotentialClientDto
            {
                BusinessInfo = new BusinessInfo
                {
                    TradeName = "Nuevo Nombre",
                    Email = "nuevo@email.com"
                },
                Status = "Pendiente"
            };

            _mockRepository.Setup(r => r.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync(existingClient);
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PotentialClient> { existingClient });

            // Act
            var result = await _useCase.Execute(VALID_ID, updateDto);

            // Assert
            Assert.NotNull(result);
            _mockRepository.Verify(r => r.UpdatePotentialClient(VALID_ID, It.IsAny<PotentialClient>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Execute_ConIdInvalido_DebeLanzarArgumentException(string invalidId)
        {
            // Arrange
            var updateDto = new UpdatePotentialClientDto();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(invalidId, updateDto));
        }

        [Fact]
        public async Task Execute_ConClienteInexistente_DebeLanzarKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync((PotentialClient)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _useCase.Execute(VALID_ID, new UpdatePotentialClientDto()));
        }

        [Fact]
        public async Task Execute_ConEmailDuplicado_DebeLanzarInvalidOperationException()
        {
            // Arrange
            var existingClient = CreateExistingClient();
            var otherClient = CreateExistingClient();
            otherClient.Id = "other_id";
            otherClient.BusinessInfo.Email = "otro@email.com";

            var updateDto = new UpdatePotentialClientDto
            {
                BusinessInfo = new BusinessInfo { Email = "otro@email.com" }
            };

            _mockRepository.Setup(r => r.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync(existingClient);
            _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PotentialClient> { existingClient, otherClient });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.Execute(VALID_ID, updateDto));
        }

        [Theory]
        [InlineData("Estado Inválido")]
        [InlineData("")]
        [InlineData(null)]
        public async Task Execute_ConEstadoInvalido_DebeLanzarArgumentException(string estadoInvalido)
        {
            // Arrange
            var existingClient = CreateExistingClient();
            var updateDto = new UpdatePotentialClientDto { Status = estadoInvalido };

            _mockRepository.Setup(r => r.GetByIdPotencialClient(VALID_ID))
                .ReturnsAsync(existingClient);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(VALID_ID, updateDto));
        }
    }
}