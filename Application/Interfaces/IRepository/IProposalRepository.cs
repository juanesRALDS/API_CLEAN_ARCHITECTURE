using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface IProposalRepository
{
    Task<(List<Proposal> Proposals, int TotalCount)> GetAllProposals(int pageNumber, int pageSize);
    Task<Proposal> GetProposalById(string id);
    Task<bool> CreateProposal(Proposal proposal);
    Task<bool> UpdateProposal(string id, Proposal proposal);
    Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateProposalSite(string proposalId, Site site);
    Task<bool> HasExistingSite(string proposalId);
}