using Moq;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.UseCases.SiteUseCase;

namespace SagaAserhi.Tests.UseCase.SiteUseCase
{
    public class GetSiteUseCaseTest
    {
        private readonly Mock<ISiteRepository> _siteRepositoryMock;
        private readonly GetSiteUseCase _useCase;
        private readonly DateTime _testDate;

        public GetSiteUseCaseTest()
        {
            _siteRepositoryMock = new Mock<ISiteRepository>();
            _useCase = new GetSiteUseCase(_siteRepositoryMock.Object);
            _testDate = DateTime.UtcNow;
        }

        private Site CreateTestSite(string id = "1") => new()
        {
            Id = id,
            Name = "Test Site",
            Address = "Test Address",
            City = "Test City",
            Phone = "1234567890",
            ProposalId = "testProposalId",
            CreatedAt = _testDate,
            Wastes = new List<Waste>()
        };

        [Fact]
        public async Task Execute_ConSitiosExistentes_DebeRetornarListaDeSitios()
        {
            // Arrange
            var proposalId = "testProposalId";
            var sites = new List<Site> { CreateTestSite() };

            _siteRepositoryMock.Setup(x => x.GetByProposalIdAsync(proposalId))
                .ReturnsAsync(sites);

            // Act
            var result = await _useCase.Execute(proposalId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var site = result.First();
            site.Id.Should().Be("1");
            site.Name.Should().Be("Test Site");
            site.Address.Should().Be("Test Address");
            site.City.Should().Be("Test City");
            site.Phone.Should().Be("1234567890");
            site.CreatedAt.Should().Be(_testDate);
        }

        [Fact]
        public async Task Execute_SinSitios_DebeRetornarListaVacia()
        {
            // Arrange
            var proposalId = "testProposalId";
            _siteRepositoryMock.Setup(x => x.GetByProposalIdAsync(proposalId))
                .ReturnsAsync(new List<Site>());

            // Act
            var result = await _useCase.Execute(proposalId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Execute_ConIdPropuestaInvalido_DebeLanzarArgumentException(string proposalId)
        {
            // Act
            Func<Task> act = () => _useCase.Execute(proposalId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("El ID de propuesta es requerido");
        }

        [Fact]
        public async Task Execute_CuandoRepositorioFalla_DebeLanzarException()
        {
            // Arrange
            var proposalId = "testProposalId";
            _siteRepositoryMock.Setup(x => x.GetByProposalIdAsync(proposalId))
                .ThrowsAsync(new Exception("Error de base de datos"));

            // Act
            Func<Task> act = () => _useCase.Execute(proposalId);

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }
    }
}