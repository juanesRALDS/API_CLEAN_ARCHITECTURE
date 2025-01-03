using Moq;
using Xunit;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.UseCases.ProposalsUseCase;

namespace SagaAserhi.Tests.UseCase.ProposalUseCase
{
    public class GetAllProposalsUseCaseTest
    {
        private readonly Mock<IProposalRepository> _mockRepository;
        private readonly GetAllProposalsUseCase _useCase;

        public GetAllProposalsUseCaseTest()
        {
            _mockRepository = new Mock<IProposalRepository>();
            _useCase = new GetAllProposalsUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_WithValidParameters_ShouldReturnProposals()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;

            List<Proposal> mockProposals = new()
            {
                new Proposal
                {
                    Id = "1",
                    Title = "Propuesta Test",
                    Description = "Descripción Test",
                    Amount = 1000M,
                    Status = "Pendiente",
                    CreationDate = DateTime.UtcNow,
                    PotentialClientId = "client1",
                    CompanyBusinessName = "Empresa Test"
                }
            };

            _mockRepository.Setup(r => r.GetAllProposals(pageNumber, pageSize))
                          .ReturnsAsync(mockProposals);

            // Act
            List<ProposalDto> result = await _useCase.Execute(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(mockProposals[0].Id, result[0].Id);
            Assert.Equal(mockProposals[0].Title, result[0].Title);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task Execute_WithInvalidPageNumber_ShouldThrowArgumentException(int pageNumber, int pageSize)
        {
            // Act & Assert
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _useCase.Execute(pageNumber, pageSize)
            );
            Assert.Contains("Page number must be greater than 0", exception.Message);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public async Task Execute_WithInvalidPageSize_ShouldThrowArgumentException(int pageNumber, int pageSize)
        {
            // Act & Assert
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _useCase.Execute(pageNumber, pageSize)
            );
            Assert.Contains("Page size must be greater than 0", exception.Message);
        }

        [Fact]
        public async Task Execute_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProposals(It.IsAny<int>(), It.IsAny<int>()))
                          .ThrowsAsync(new Exception("Error de repositorio"));

            // Act & Assert
            Exception exception = await Assert.ThrowsAsync<Exception>(
                async () => await _useCase.Execute(1, 10)
            );
            Assert.Contains("Error al obtener propuestas", exception.Message);
        }
    }
}