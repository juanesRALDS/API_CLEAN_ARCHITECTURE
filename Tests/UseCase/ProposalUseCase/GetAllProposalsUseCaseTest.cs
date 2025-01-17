using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.UseCases.ProposalsUseCase;

namespace SagaAserhi.Tests.UseCase.ProposalUseCase
{
    public class GetAllProposalsUseCaseTest
    {
        private readonly Mock<IProposalRepository> _mockRepository;
        private readonly GetAllProposalsUseCase _useCase;
        private readonly DateTime _testDate;

        public GetAllProposalsUseCaseTest()
        {
            _mockRepository = new Mock<IProposalRepository>();
            _useCase = new GetAllProposalsUseCase(_mockRepository.Object);
            _testDate = DateTime.UtcNow;
        }

        [Fact]
        public async Task Execute_ConParametrosValidos_DebeRetornarPropuestas()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var mockProposals = new List<Proposal>
            {
                new()
                {
                    Id = "1",
                    ClientId = "clientId1",
                    Number = "PROP-001",
                    Status = new ProposalStatus
                    {
                        Proposal = "Pendiente",
                        Sending = "No enviado",
                        Review = "Sin revisar"
                    },
                    Sites = new List<Site>(),
                    History = new List<ProposalHistory>
                    {
                        new()
                        {
                            Action = "Creación",
                            Date = _testDate,
                            PotentialClientId = "clientId1"
                        }
                    },
                    CreatedAt = _testDate,
                    UpdatedAt = _testDate,
                    PotentialClient = new PotentialClient
                    {
                        BusinessInfo = new BusinessInfo
                        {
                            TradeName = "Empresa Test"
                        }
                    }
                }
            };

            _mockRepository.Setup(repo => 
                repo.GetAllProposals(pageNumber, pageSize))
                .ReturnsAsync((mockProposals, 1));

            // Act
            var (proposals, totalCount) = await _useCase.Execute(pageNumber, pageSize);

            // Assert
            Assert.Single(proposals);
            Assert.Equal(1, totalCount);
            var proposal = proposals[0];
            Assert.Equal("1", proposal.Id);
            Assert.Equal("PROP-001", proposal.Number);
            Assert.Equal("Pendiente", proposal.Status.Proposal);
            Assert.Equal("Empresa Test", proposal.CompanyBusinessName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public async Task Execute_ConParametrosInvalidos_DebeLanzarArgumentException(
            int pageNumber, int pageSize)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(pageNumber, pageSize));

            Assert.Contains(
                pageNumber <= 0 ? "número de página" : "tamaño de página", 
                exception.Message.ToLower());
        }

        [Fact]
        public async Task Execute_CuandoRepositorioFalla_DebePropararError()
        {
            // Arrange
            _mockRepository.Setup(repo => 
                repo.GetAllProposals(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Error de repositorio"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _useCase.Execute(1, 10));
            
            Assert.Contains("Error al obtener propuestas", exception.Message);
        }
    }
}