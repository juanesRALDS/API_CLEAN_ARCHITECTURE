// Infrastructure/Repositories/ContractRepository.cs
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly IMongoCollection<Contract> _contractCollection;

    public ContractRepository(MongoDbContext context)
    {
        _contractCollection = context.GetCollection<Contract>("Contracts");
    }

    public async Task<(List<Contract>, int)> GetAllContracts(int pageNumber, int pageSize)
    {
        try
        {
            var filter = Builders<Contract>.Filter.Empty;
            var sort = Builders<Contract>.Sort.Descending(x => x.CreatedAt);

            var contracts = await _contractCollection
                .Find(filter)
                .Sort(sort)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var totalCount = await _contractCollection.CountDocumentsAsync(filter);

            return (contracts, (int)totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener contratos: {ex.Message}", ex);
        }
    }

    public async Task<bool> CreateContract(Contract contract)
    {
        try
        {
            await _contractCollection.InsertOneAsync(contract);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Contract?> GetContractById(string id)
    {
        return await _contractCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Contract>> GetContractsByProposalId(string proposalId)
    {
        return await _contractCollection.Find(x => x.ProposalId == proposalId).ToListAsync();
    }
}