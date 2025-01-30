using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.UseCases.ProposalsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.UseCase.ProposalUseCase
{
    public class UpdateProposalUseCaseTest
    {
        private readonly Mock<IProposalRepository> _mockRepository;
        private readonly UpdateProposalUseCase _useCase;
        private readonly string _validProposalId;
        private readonly UpdateProposalDto _validUpdateDto;
        private readonly Proposal _existingProposal;
        private readonly DateTime _testDate;

        public UpdateProposalUseCaseTest()
        {
            _mockRepository = new Mock<IProposalRepository>();
            _useCase = new UpdateProposalUseCase(_mockRepository.Object);
            _testDate = DateTime.UtcNow;
            _validProposalId = "valid_id";
            
            _validUpdateDto = new UpdateProposalDto
            {
                Status = new UpdateProposalStatusDto
                {
                    Proposal = "Aprobada",
                    Sending = "Enviado",
                    Review = "Revisado"
                }
            };

            _existingProposal = new Proposal
            {
                Id = _validProposalId,
                Number = "PROP-001",
                Status = new ProposalStatus
                {
                    Proposal = "Pendiente",
                    Sending = "No enviado",
                    Review = "Sin revisar"
                },
                CreatedAt = _testDate.AddDays(-1),
                UpdatedAt = _testDate.AddDays(-1)
            };
        }

        [Fact]
        public async Task Execute_ConDatosValidos_DebeActualizarPropuesta()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetProposalById(_validProposalId))
                .ReturnsAsync(_existingProposal);
            
            _mockRepository.Setup(repo => repo.UpdateProposal(_validProposalId, It.IsAny<Proposal>()))
                .ReturnsAsync(true);

            // Act
            var result = await _useCase.Execute(_validProposalId, _validUpdateDto);

            // Assert
            Assert.Equal("Propuesta actualizada exitosamente", result);
            _mockRepository.Verify(repo => repo.UpdateProposal(_validProposalId, 
                It.Is<Proposal>(p => 
                    p.Status.Proposal == _validUpdateDto.Status.Proposal &&
                    p.Status.Sending == _validUpdateDto.Status.Sending &&
                    p.Status.Review == _validUpdateDto.Status.Review
                )), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task Execute_ConIdInvalido_DebeLanzarArgumentException(string invalidId)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _useCase.Execute(invalidId, _validUpdateDto));
        }

        [Fact]
        public async Task Execute_ConPropuestaInexistente_DebeLanzarInvalidOperationException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetProposalById(_validProposalId))
                .ReturnsAsync((Proposal)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.Execute(_validProposalId, _validUpdateDto));
        }

        [Fact]
        public async Task Execute_CuandoFallaActualizacion_DebeLanzarInvalidOperationException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetProposalById(_validProposalId))
                .ReturnsAsync(_existingProposal);
            
            _mockRepository.Setup(repo => repo.UpdateProposal(_validProposalId, It.IsAny<Proposal>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _useCase.Execute(_validProposalId, _validUpdateDto));
        }

        [Fact]
        public async Task Execute_DebeActualizarFechaModificacion()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetProposalById(_validProposalId))
                .ReturnsAsync(_existingProposal);
            
            _mockRepository.Setup(repo => repo.UpdateProposal(_validProposalId, It.IsAny<Proposal>()))
                .ReturnsAsync(true);

            // Act
            await _useCase.Execute(_validProposalId, _validUpdateDto);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateProposal(_validProposalId, 
                It.Is<Proposal>(p => p.UpdatedAt > _existingProposal.UpdatedAt)), 
                Times.Once);
        }
    }
}