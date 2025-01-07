using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface IPotentialClientRepository
{
    Task<List<PotentialClient>> GetAllPotentialClientsWithProposals(int pageNumber, int pageSize);
    Task CreatePotentialClient(PotentialClient client);
    Task<PotentialClient?> GetByIdPotencialClient(string id);
    Task DeletePoTencialClient(string Id);
    Task UpdatePotentialClient(string Id, PotentialClient client);
    Task<bool> AddProposalToPotentialClient(string clientId, Proposal proposal);

    Task<IEnumerable<PotentialClient>> GetAllAsync(CancellationToken cancellationToken);

}