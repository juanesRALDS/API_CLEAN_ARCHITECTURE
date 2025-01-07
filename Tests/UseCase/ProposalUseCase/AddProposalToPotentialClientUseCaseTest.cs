using Moq;
using Xunit;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.UseCases.PotentialClientsUseCa;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.UseCase.ProposalUseCase
{
    public class AddProposalToPotentialClientUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly AddProposalToPotentialClientUseCase _useCase;
        
        private const string VALID_CLIENT_ID = "valid_client_id";
        private const string PROPOSAL_TITLE = "Test Proposal";
        private const string PROPOSAL_DESCRIPTION = "Test Description";
        private const decimal PROPOSAL_AMOUNT = 1000.00M;

        public AddProposalToPotentialClientUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new AddProposalToPotentialClientUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_WithValidData_ShouldAddProposalSuccessfully()
        {
            // Arrange
            PotentialClient existingClient = new()
            {
                Id = VALID_CLIENT_ID,
                CompanyBusinessName = "Test Company"
            };

            CreateProposalDto proposalDto = new()
            {
                Title = PROPOSAL_TITLE,
                Description = PROPOSAL_DESCRIPTION,
                Amount = PROPOSAL_AMOUNT
            };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(VALID_CLIENT_ID))
                .ReturnsAsync(existingClient);
            _mockRepository.Setup(repo => repo.AddProposalToPotentialClient(VALID_CLIENT_ID, It.IsAny<Proposal>()))
                .ReturnsAsync(true);

            // Act
            string result = await _useCase.Execute(VALID_CLIENT_ID, proposalDto);

            // Assert
            Assert.Equal("Propuesta agregada exitosamente", result);
            _mockRepository.Verify(repo => repo.AddProposalToPotentialClient(VALID_CLIENT_ID, 
                It.Is<Proposal>(p => 
                    p.Title == PROPOSAL_TITLE && 
                    p.Description == PROPOSAL_DESCRIPTION && 
                    p.Amount == PROPOSAL_AMOUNT &&
                    p.Status == "Pendiente")), 
                Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Execute_WithInvalidClientId_ShouldThrowArgumentException(string invalidClientId)
        {
            // Arrange
            CreateProposalDto proposalDto = new()
            {
                Title = PROPOSAL_TITLE,
                Description = PROPOSAL_DESCRIPTION,
                Amount = PROPOSAL_AMOUNT
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(invalidClientId, proposalDto));
        }

        [Fact]
        public async Task Execute_WithNullProposalDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _useCase.Execute(VALID_CLIENT_ID, null!));
        }

        [Fact]
        public async Task Execute_WithNonExistentClient_ShouldThrowInvalidOperationException()
        {
            // Arrange
            CreateProposalDto proposalDto = new()
            {
                Title = PROPOSAL_TITLE,
                Description = PROPOSAL_DESCRIPTION,
                Amount = PROPOSAL_AMOUNT
            };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(VALID_CLIENT_ID))
                .ReturnsAsync((PotentialClient)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.Execute(VALID_CLIENT_ID, proposalDto));
        }

        [Fact]
        public async Task Execute_WhenAddProposalFails_ShouldThrowInvalidOperationException()
        {
            // Arrange
            PotentialClient existingClient = new()
            {
                Id = VALID_CLIENT_ID,
                CompanyBusinessName = "Test Company"
            };

            CreateProposalDto proposalDto = new()
            {
                Title = PROPOSAL_TITLE,
                Description = PROPOSAL_DESCRIPTION,
                Amount = PROPOSAL_AMOUNT
            };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(VALID_CLIENT_ID))
                .ReturnsAsync(existingClient);
            _mockRepository.Setup(repo => repo.AddProposalToPotentialClient(VALID_CLIENT_ID, It.IsAny<Proposal>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.Execute(VALID_CLIENT_ID, proposalDto));
        }
    }
}