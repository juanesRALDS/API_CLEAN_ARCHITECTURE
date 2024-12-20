using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces;

public interface IPotentialClientRepository
{
    Task<List<PotentialClient>> GetAllPotentialClients(int pageNumber, int pageSize);
    Task CreatePotentialClient(PotentialClient client);
}