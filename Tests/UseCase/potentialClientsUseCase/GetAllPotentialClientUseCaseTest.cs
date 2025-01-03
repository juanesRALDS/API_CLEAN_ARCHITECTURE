using Moq;
using Xunit;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase
{
    public class GetAllPotentialClientsWithProposalsUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly GetAllPotentialClientsWithProposalsUseCase _useCase;

        public GetAllPotentialClientsWithProposalsUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new GetAllPotentialClientsWithProposalsUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_WithValidParameters_ShouldReturnClientList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            List<PotentialClient> potentialClients = new List<PotentialClient>
            {
                new PotentialClient
                {
                    Id = "1",
                    CompanyBusinessName = "Test Company",
                    ContactPhone = "123456789",
                    ContactEmail = "test@test.com",
                    Proposals = new List<string> { "proposal1", "proposal2" }
                }
            };

            _mockRepository.Setup(repo => repo.GetAllPotentialClientsWithProposals(pageNumber, pageSize))
                .ReturnsAsync(potentialClients);

            // Act
            List<PotentialClientDto> result = await _useCase.Execute(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1", result[0].Id);
            Assert.Equal("Test Company", result[0].CompanyBusinessName);
            Assert.Equal(2, result[0].Proposals.Count);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        [InlineData(-1, 10)]
        [InlineData(1, -1)]
        public async Task Execute_WithInvalidParameters_ShouldThrowArgumentException(int pageNumber, int pageSize)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(pageNumber, pageSize));
        }

        [Fact]
        public async Task Execute_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            _mockRepository.Setup(repo => repo.GetAllPotentialClientsWithProposals(pageNumber, pageSize))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => 
                _useCase.Execute(pageNumber, pageSize));
        }

        [Fact]
        public async Task Execute_WithEmptyProposals_ShouldReturnEmptyProposalsList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            List<PotentialClient> potentialClients = new List<PotentialClient>
            {
                new PotentialClient
                {
                    Id = "1",
                    CompanyBusinessName = "Test Company",
                    ContactPhone = "123456789",
                    ContactEmail = "test@test.com",
                    Proposals = null!
                }
            };

            _mockRepository.Setup(repo => repo.GetAllPotentialClientsWithProposals(pageNumber, pageSize))
                .ReturnsAsync(potentialClients);

            // Act
            List<PotentialClientDto> result = await _useCase.Execute(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Empty(result[0].Proposals);
        }
    }
}