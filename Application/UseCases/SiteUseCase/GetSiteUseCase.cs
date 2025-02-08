using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.ISiteUseCase;
using SagaAserhi.Domain.Entities;
namespace SagaAserhi.Application.UseCases.SiteUseCase
{
    public class GetSiteUseCase : IGetSiteUseCase
    {
        private readonly ISiteRepository _siteRepository;

        public GetSiteUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<List<SiteDtos>> Execute(string proposalId, int pageNumber, int pageSize)
        {
            IEnumerable<Site> sites;

            if (string.IsNullOrWhiteSpace(proposalId))
            {
                sites = await _siteRepository.GetAllSites(pageNumber, pageSize);
            }
            else
            {
                sites = await _siteRepository.GetByProposalId(proposalId, pageNumber, pageSize);
            }

            return sites.Select(site => new SiteDtos
            {
                Id = site.Id,
                Name = site.Name,
                Address = site.Address,
                City = site.City,
                Department = site.Department,
                Phone = site.Phone,
                ProposalId = site.ProposalId,
                ClientID = site.ClientID,
                Wastes = site.Wastes.Select(w => new WasteDto
                {
                    Type = w.Type,
                    Classification = w.Classification,
                    Treatment = w.Treatment,
                    Price = w.Price,
                    DescriptionWaste = w.DescriptionWaste
                }).ToList(),
                Frequency = new FrequencyDto
                {
                    FrequencyOfTravel = site.Frequencies.FrequencyOfTravel,
                    Amount = site.Frequencies.Amount
                },
                TotalPrice = site.TotalPrice,
                CreatedAt = site.CreatedAt
            }).ToList();
        }
    }
}
