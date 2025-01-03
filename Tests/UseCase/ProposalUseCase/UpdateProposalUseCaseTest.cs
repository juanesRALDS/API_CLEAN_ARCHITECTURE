using Moq;
using Xunit;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.UseCases.ProposalsUseCase;

namespace SagaAserhi.Tests.UseCase.ProposalUseCase
{
    public class UpdateProposalUseCaseTest
    {
        private readonly Mock<IProposalRepository> _mockRepository;
        private readonly UpdateProposalUseCase _useCase;
        private readonly string _validProposalId;
        private readonly UpdateProposalDto _validUpdateDto;
        private readonly Proposal _existingProposal;

        public UpdateProposalUseCaseTest()
        {
            _mockRepository = new Mock<IProposalRepository>();
            _useCase = new UpdateProposalUseCase(_mockRepository.Object);
            
            _validProposalId = "valid-id";
            _validUpdateDto = new UpdateProposalDto
            {
                Title = "Título Actualizado",
                Description = "Descripción Actualizada",
                Amount = 2000M,
                Status = "Actualizado"
            };
            _existingProposal = new Proposal
            {
                Id = _validProposalId,
                Title = "Título Original",
                Description = "Descripción Original",
                Amount = 1000M,
                Status = "Pendiente"
            };
        }

        [Fact]
        public async Task Execute_WithValidData_ShouldUpdateProposalSuccessfully()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetProposalById(_validProposalId))
                          .ReturnsAsync(_existingProposal);
            _mockRepository.Setup(r => r.UpdateProposal(_validProposalId, It.IsAny<Proposal>()))
                          .ReturnsAsync(true);

            // Act
            string result = await _useCase.Execute(_validProposalId, _validUpdateDto);

            // Assert
            Assert.Equal("Propuesta actualizada exitosamente", result);
            _mockRepository.Verify(r => r.UpdateProposal(_validProposalId, It.IsAny<Proposal>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Execute_WithInvalidId_ShouldThrowArgumentException(string invalidId)
        {
            // Act & Assert
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _useCase.Execute(invalidId, _validUpdateDto)
            );
            Assert.Equal("El ID es requerido", exception.Message);
        }

        [Fact]
        public async Task Execute_WithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _useCase.Execute(_validProposalId, null!)
            );
        }

        [Fact]
        public async Task Execute_WithNonExistentProposal_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetProposalById(_validProposalId))
                          .ReturnsAsync((Proposal)null!);

            // Act & Assert
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _useCase.Execute(_validProposalId, _validUpdateDto)
            );
            Assert.Contains($"No se encontró la propuesta con ID: {_validProposalId}", exception.Message);
        }

        [Fact]
        public async Task Execute_WhenUpdateFails_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetProposalById(_validProposalId))
                          .ReturnsAsync(_existingProposal);
            _mockRepository.Setup(r => r.UpdateProposal(_validProposalId, It.IsAny<Proposal>()))
                          .ReturnsAsync(false);

            // Act & Assert
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _useCase.Execute(_validProposalId, _validUpdateDto)
            );
            Assert.Equal("No se pudo actualizar la propuesta", exception.Message);
        }
    }
}