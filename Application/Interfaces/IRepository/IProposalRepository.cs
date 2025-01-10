using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface IProposalRepository
{
    Task<List<Proposal>> GetAllProposals(int pageNumber, int pageSize);
    Task<bool> UpdateProposal(string id, Proposal proposal);
    Task<Proposal> GetProposalById(string id);
    Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken);

    Task<bool> UpdateProposalSite(string proposalId, string siteId);
    Task<bool> HasExistingSite(string proposalId);
}