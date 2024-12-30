using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces;

public interface IProposalRepository
{
    Task<List<Domain.Entities.Proposal>> GetAllProposals(int pageNumber, int pageSize);
    Task<bool> UpdateProposal(string id, Domain.Entities.Proposal proposal);
    Task <Domain.Entities.Proposal> GetProposalById(string id);
}