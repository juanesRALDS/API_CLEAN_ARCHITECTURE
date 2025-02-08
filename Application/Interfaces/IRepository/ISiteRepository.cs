using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface ISiteRepository
{
    Task CreateSite(Site site);
    Task<Site> GetByIdSite(string id);
    Task<IEnumerable<Site>> GetByProposalId(string proposalId, int pageNumber = 1, int pageSize = 10);
     Task<IEnumerable<Site>> GetAllSites(int pageNumber = 1, int pageSize = 10);
    Task<Site> UpdateSite(string id, Site site);
    Task DeleteSite(string id);
    Task<bool> ExistsSite(string id);
}