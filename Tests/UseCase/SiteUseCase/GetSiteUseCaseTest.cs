using FluentAssertions;
using Moq;
using Xunit;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.UseCases.SiteUseCase;
using SagaAserhi.Application.Interfaces.ISiteUseCase;

namespace SagaAserhi.Tests.UseCase.Sites;

public class GetSiteUseCaseTests
{
    private readonly Mock<ISiteRepository> _siteRepositoryMock;
    private readonly IGetSiteUseCase _useCase;

    public GetSiteUseCaseTests()
    {
        _siteRepositoryMock = new Mock<ISiteRepository>();
        _useCase = new GetSiteUseCase(_siteRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedSites_WhenSitesExist()
    {
        // Arrange
        var proposalId = "123";
        var sites = new List<Site>
        {
            new()
            {
                Id = "1",
                Name = "Test Site",
                Address = "Test Address",
                City = "Test City",
                Phone = "1234567890",
                Status = true,
                CreatedAt = DateTime.UtcNow,
                ProposalId = proposalId
            }
        };

        _siteRepositoryMock.Setup(x => x.GetByProposalIdAsync(proposalId))
            .ReturnsAsync(sites);

        // Act
        var result = await _useCase.Execute(proposalId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var siteDto = result.First();
        siteDto.Id.Should().Be(sites[0].Id);
        siteDto.Name.Should().Be(sites[0].Name);
        siteDto.Address.Should().Be(sites[0].Address);
        siteDto.City.Should().Be(sites[0].City);
        siteDto.Phone.Should().Be(sites[0].Phone);
        siteDto.Status.Should().Be(sites[0].Status);
        siteDto.CreatedAt.Should().Be(sites[0].CreatedAt);
        siteDto.ProposalId.Should().Be(sites[0].ProposalId);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoSitesExist()
    {
        // Arrange
        var proposalId = "123";
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
    public async Task Execute_ShouldThrowArgumentException_WhenProposalIdIsInvalid(string proposalId)
    {
        // Act
        Func<Task> act = () => _useCase.Execute(proposalId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("El ID de propuesta es requerido");
    }

    [Fact]
    public async Task Execute_ShouldThrowApplicationException_WhenRepositoryFails()
    {
        // Arrange
        var proposalId = "123";
        _siteRepositoryMock.Setup(x => x.GetByProposalIdAsync(proposalId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = () => _useCase.Execute(proposalId);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");
    }
}