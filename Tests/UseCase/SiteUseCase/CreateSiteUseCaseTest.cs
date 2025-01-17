using FluentAssertions;
using Moq;
using Xunit;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Application.UseCases.SiteUseCase;

namespace SagaAserhi.Tests.UseCase.Sites;

public class CreateSiteUseCaseTests
{
    private readonly Mock<ISiteRepository> _siteRepositoryMock;
    private readonly Mock<IProposalRepository> _proposalRepositoryMock;
    private readonly CreateSiteUseCase _useCase;

    public CreateSiteUseCaseTests()
    {
        _siteRepositoryMock = new Mock<ISiteRepository>();
        _proposalRepositoryMock = new Mock<IProposalRepository>();
        _useCase = new CreateSiteUseCase(_siteRepositoryMock.Object, _proposalRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateSite_WhenValidRequestProvided()
    {
        // Arrange
        string proposalId = "123";
        CreateSiteDto siteInfo = new()
        {
            Name = "Test Site",
            Address = "Test Address",
            City = "Test City",
            Phone = "1234567890"
        };
        SiteRequestDto request = new() { ProposalId = proposalId, SiteInfo = siteInfo };

        _proposalRepositoryMock.Setup(x => x.GetProposalById(proposalId))
            .ReturnsAsync(new Proposal { Id = proposalId });
        _proposalRepositoryMock.Setup(x => x.HasExistingSite(proposalId))
            .ReturnsAsync(false);

        // Act
        SiteDtos result = await _useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(siteInfo.Name.Trim());
        result.Address.Should().Be(siteInfo.Address.Trim());
        result.City.Should().Be(siteInfo.City.Trim());
        result.Phone.Should().Be(siteInfo.Phone.Trim());

        _siteRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Site>()), Times.Once);
        _proposalRepositoryMock.Verify(x => x.UpdateProposalSite(proposalId, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _useCase.Execute(null!));

        exception.ParamName.Should().Be("request");
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenProposalNotFound()
    {
        // Arrange
        SiteRequestDto request = new()
        {
            ProposalId = "nonexistent",
            SiteInfo = new CreateSiteDto()
        };
        _proposalRepositoryMock.Setup(x => x.GetProposalById(request.ProposalId))
            .ReturnsAsync((Proposal)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _useCase.Execute(null!));
    }

    [Fact]
    public async Task Execute_ShouldThrowInvalidOperationException_WhenSiteAlreadyExists()
    {
        // Arrange
        SiteRequestDto request = new()
        {
            ProposalId = "123",
            SiteInfo = new CreateSiteDto()
        };
        _proposalRepositoryMock.Setup(x => x.GetProposalById(request.ProposalId))
            .ReturnsAsync(new Proposal { Id = request.ProposalId });
        _proposalRepositoryMock.Setup(x => x.HasExistingSite(request.ProposalId))
            .ReturnsAsync(true);

        // Act & Assert
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _useCase.Execute(request));
        exception.Message.Should().Be("Esta propuesta ya tiene un sitio asignado");
    }

    [Fact]
    public async Task Execute_ShouldTrimsiteInfoFields()
    {
        // Arrange
        SiteRequestDto request = new()
        {
            ProposalId = "123",
            SiteInfo = new CreateSiteDto
            {
                Name = "  Test Site  ",
                Address = "  Test Address  ",
                City = "  Test City  ",
                Phone = "  1234567890  "
            }
        };

        _proposalRepositoryMock.Setup(x => x.GetProposalById(request.ProposalId))
            .ReturnsAsync(new Proposal { Id = request.ProposalId });
        _proposalRepositoryMock.Setup(x => x.HasExistingSite(request.ProposalId))
            .ReturnsAsync(false);

        // Act
        SiteDtos result = await _useCase.Execute(request);

        // Assert
        result.Name.Should().Be("Test Site");
        result.Address.Should().Be("Test Address");
        result.City.Should().Be("Test City");
        result.Phone.Should().Be("1234567890");
    }

    [Fact]
    public async Task Execute_ShouldThrowApplicationException_WhenRepositoryThrowsException()
    {
        // Arrange
        SiteRequestDto request = new()
        {
            ProposalId = "123",
            SiteInfo = new CreateSiteDto()
        };

        _proposalRepositoryMock.Setup(x => x.GetProposalById(request.ProposalId))
            .ReturnsAsync(new Proposal { Id = request.ProposalId });
        _siteRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Site>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = () => _useCase.Execute(request);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("Error al crear la sede: Database error");
    }
}