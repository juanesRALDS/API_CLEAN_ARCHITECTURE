using Moq;
using Xunit;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase
{
    public class DeletePotentialClientUseCaseTest
    {
        private readonly Mock<IPotentialClientRepository> _mockRepository;
        private readonly DeletePotentialClientUseCase _useCase;

        public DeletePotentialClientUseCaseTest()
        {
            _mockRepository = new Mock<IPotentialClientRepository>();
            _useCase = new DeletePotentialClientUseCase(_mockRepository.Object);
        }

        [Fact]
        public async Task Execute_WithValidId_ShouldDeleteAndReturnSuccessMessage()
        {
            // Arrange
            string potentialClientId = "valid_id";
            PotentialClient existingClient = new PotentialClient { Id = potentialClientId };

            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(potentialClientId))
                .ReturnsAsync(existingClient);
            _mockRepository.Setup(repo => repo.DeletePoTencialClient(potentialClientId))
                .Returns(Task.CompletedTask);

            // Act
            string result = await _useCase.Execute(potentialClientId);

            // Assert
            Assert.Equal("Cliente potencial eliminado exitosamente", result);
            _mockRepository.Verify(repo => repo.DeletePoTencialClient(potentialClientId), Times.Once);
        }

        [Fact]
        public async Task Execute_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            string invalidId = "invalid_id";
            _mockRepository.Setup(repo => repo.GetByIdPotencialClient(invalidId))
                .ReturnsAsync((PotentialClient)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _useCase.Execute(invalidId)
            );

            Assert.Contains(invalidId, exception.Message);
            _mockRepository.Verify(repo => repo.GetByIdPotencialClient(invalidId), Times.Once);
            _mockRepository.Verify(repo => repo.DeletePoTencialClient(invalidId), Times.Never);
        }
    }
}