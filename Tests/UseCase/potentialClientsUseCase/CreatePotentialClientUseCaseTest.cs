using Xunit;
using Moq;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase;

public class CreatePotentialClientUseCaseTest
{
    private readonly Mock<IPotentialClientRepository> _mockRepository;
    private readonly CreatePotentialClientUseCase _useCase;

    public CreatePotentialClientUseCaseTest()
    {
        _mockRepository = new Mock<IPotentialClientRepository>();
        _useCase = new CreatePotentialClientUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task Execute_WithValidDto_ShouldReturnSuccessMessage()
    {
        // Arrange
        CreatePotentialClientDto? dto = new()
        {
            CompanyBusinessName = "Test Company",
            ContactPhone = "1234567890",
            ContactEmail = "test@test.com"
        };

        _mockRepository
            .Setup(x => x.CreatePotentialClient(It.IsAny<PotentialClient>()))
            .Returns(Task.CompletedTask);

        // Act
        string? result = await _useCase.Execute(dto);

        // Assert
        Assert.Equal("Potential client created successfully", result);
        _mockRepository.Verify(x => x.CreatePotentialClient(It.IsAny<PotentialClient>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithNullDto_ShouldThrowArgumentNullException()
    {
        // Arrange
        CreatePotentialClientDto? dto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _useCase.Execute(dto!));
    }

    [Fact]
    public async Task Execute_WithEmptyCompanyName_ShouldThrowArgumentException()
    {
        // Arrange
        CreatePotentialClientDto? dto = new()
        {
            CompanyBusinessName = "",
            ContactPhone = "1234567890",
            ContactEmail = "test@test.com"
        };

        // Act & Assert
        ArgumentException? exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(dto));
        Assert.Equal("Company name cannot be empty (Parameter 'CompanyBusinessName')", exception.Message);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrows_ShouldPropagateException()
    {
        // Arrange
        CreatePotentialClientDto? dto = new()
        {
            CompanyBusinessName = "Test Company",
            ContactPhone = "1234567890",
            ContactEmail = "test@test.com"
        };

        _mockRepository
            .Setup(x => x.CreatePotentialClient(It.IsAny<PotentialClient>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(dto));
    }
}