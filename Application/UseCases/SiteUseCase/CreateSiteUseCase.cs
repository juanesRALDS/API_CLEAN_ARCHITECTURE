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

        request.SiteInfo.Validate();

        try
        {
            var proposal = await _proposalRepository.GetProposalById(request.ProposalId)
                ?? throw new InvalidOperationException($"Propuesta no encontrada con ID: {request.ProposalId}");

            var site = new Site
            {
                Name = request.SiteInfo.Name,
                Address = request.SiteInfo.Address,
                City = request.SiteInfo.City,
                Phone = request.SiteInfo.Phone,
                ProposalId = request.ProposalId,
                CreatedAt = DateTime.UtcNow,
                Wastes = new List<Waste>()
            };

            // Crear el sitio y actualizar la propuesta
            await _siteRepository.CreateAsync(site);
            var updated = await _proposalRepository.UpdateProposalSite(request.ProposalId, site);

            if (!updated)
                throw new InvalidOperationException("No se pudo actualizar la propuesta con el nuevo sitio");

            return new SiteDtos
            {
                Id = site.Id,
                Name = site.Name,
                Address = site.Address,
                City = site.City,
                Phone = site.Phone,
                CreatedAt = site.CreatedAt
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al crear el sitio: {ex.Message}", ex);
        }
    }
}