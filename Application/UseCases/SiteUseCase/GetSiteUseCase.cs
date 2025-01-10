using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.ISiteUseCase;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.SiteUseCase;

public class GetSiteUseCase : IGetSiteUseCase
{
    private readonly ISiteRepository _siteRepository;

    public GetSiteUseCase(ISiteRepository siteRepository)
    {
        _siteRepository = siteRepository;
    }

    public async Task<List<SiteDtos>> Execute(string proposalId)
    {
        if (string.IsNullOrWhiteSpace(proposalId))
            throw new ArgumentException("El ID de propuesta es requerido");
        
        IEnumerable<Site>? sites = await _siteRepository.GetByProposalIdAsync(proposalId);

        return sites.Select(site => new SiteDtos
        {
            Id = site.Id,
            Name = site.Name,
            Address = site.Address,
            City = site.City,
            Phone = site.Phone,
            Status = site.Status,
            CreatedAt = site.CreatedAt,
            ProposalId = site.ProposalId
        }).ToList();
    }
}