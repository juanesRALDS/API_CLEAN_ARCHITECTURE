using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Tests.UseCase.PotentialClientsUseCase;

public class GetAllPotentialClientsUseCaseTest
{
    private readonly Mock<IPotentialClientRepository> _mockRepository;
    private readonly GetAllPotentialClientsWithProposalsUseCase _useCase;
    private readonly DateTime _testDate;

    public GetAllPotentialClientsUseCaseTest()
    {
        _mockRepository = new Mock<IPotentialClientRepository>();
        _useCase = new GetAllPotentialClientsWithProposalsUseCase(_mockRepository.Object);
        _testDate = DateTime.UtcNow;
    }

    private PotentialClient CreateTestPotentialClient()
    {
        return new PotentialClient
        {
            Id = "1",
            Identification = new Identification 
            { 
                Type = "NIT", 
                Number = "123456789" 
            },
            BusinessInfo = new BusinessInfo
            {
                TradeName = "Test Company",
                EconomicActivity = "Software Development",
                Email = "test@test.com",
                Phone = "123456789"
            },
            Status = new Status
            {
                Current = "Activo",
                History = new List<StatusHistory>
                {
                    new StatusHistory
                    {
                        Status = "Activo",
                        Date = _testDate,
                        Observation = "Cliente creado"
                    }
                }
            },
            CreatedAt = _testDate,
            UpdatedAt = _testDate
        };
    }

    [Fact]
    public async Task Execute_ConDatosValidos_DebeRetornarListaClientes()
    {
        // Arrange
        var potentialClients = new List<PotentialClient> { CreateTestPotentialClient() };
        _mockRepository.Setup(repo => repo.GetAllPotentialClientsWithProposals(1, 10))
            .ReturnsAsync(potentialClients);

        // Act
        var result = await _useCase.Execute(1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var cliente = result[0];
        Assert.Equal("1", cliente.Id);
        Assert.Equal("Test Company", cliente.BusinessInfo.TradeName);
        Assert.Equal("123456789", cliente.Identification.Number);
        Assert.Equal("Activo", cliente.Status.Current);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(-1, 10)]
    [InlineData(1, -1)]
    public async Task Execute_ConPaginacionInvalida_DebeLanzarArgumentException(int pageNumber, int pageSize)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _useCase.Execute(pageNumber, pageSize));

        Assert.Contains(pageNumber <= 0 ? "Page number" : "Page size", exception.Message);
    }

    [Fact]
    public async Task Execute_CuandoRepositorioLanzaExcepcion_DebePropagerExcepcion()
    {
        // Arrange
        _mockRepository.Setup(repo => 
            repo.GetAllPotentialClientsWithProposals(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Error de repositorio"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _useCase.Execute(1, 10));
        Assert.Equal("Error al obtener clientes con propuestas", exception.Message);
    }

    [Fact]
    public async Task Execute_CuandoNoHayClientes_DebeRetornarListaVacia()
    {
        // Arrange
        _mockRepository.Setup(repo => 
            repo.GetAllPotentialClientsWithProposals(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<PotentialClient>());

        // Act
        var result = await _useCase.Execute(1, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}