using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.ISiteUseCase;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.SiteUseCase;

public class CreateSiteUseCase : ICreateSiteUseCase
{
    private readonly ISiteRepository _siteRepository;
    private readonly IProposalRepository _proposalRepository;

    public CreateSiteUseCase(ISiteRepository siteRepository, IProposalRepository proposalRepository)
    {
        _siteRepository = siteRepository;
        _proposalRepository = proposalRepository;
    }
    public async Task<SiteDtos> Execute(SiteRequestDto request)
    {
        if (request?.SiteInfo == null)
            throw new ArgumentNullException(nameof(request));

        try
        {

            Proposal? proposal = await _proposalRepository.GetProposalById(request.ProposalId)
               ?? throw new Exception($"Propuesta no encontrada con ID: {request.ProposalId}");

            if (await _proposalRepository.HasExistingSite(request.ProposalId))
                throw new InvalidOperationException("Esta propuesta ya tiene un sitio asignado");

            Site? site = new()
            {
                Name = request.SiteInfo.Name.Trim(),
                Address = request.SiteInfo.Address.Trim(),
                City = request.SiteInfo.City.Trim(),
                Phone = request.SiteInfo.Phone?.Trim() ?? string.Empty,
                ProposalId = request.ProposalId,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            await _siteRepository.CreateAsync(site);
            await _proposalRepository.UpdateProposalSite(request.ProposalId, site.Id);

            return new SiteDtos
            {
                Id = site.Id,
                Name = site.Name,
                Address = site.Address,
                City = site.City,
                Phone = site.Phone,
                Status = site.Status,
                CreatedAt = site.CreatedAt,
                ProposalId = site.ProposalId
            };
        }
        catch (Exception ex) when (ex is not ArgumentNullException
                                  && ex is not InvalidOperationException)
        {
            throw new ApplicationException($"Error al crear la sede: {ex.Message}", ex);
        }
    }
}