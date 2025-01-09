using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository
{
    public interface ISiteRepository
    {
        Task CreateAsync(Site site);
        Task<Site> GetByIdAsync(string id);
        Task<IEnumerable<Site>> GetByProposalIdAsync(string proposalId);
        Task UpdateAsync(Site site);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}