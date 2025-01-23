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
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            Console.WriteLine($"Intentando crear contrato: {contract.Id}");
            await _contractCollection.InsertOneAsync(contract);
            Console.WriteLine($"Contrato creado exitosamente: {contract.Id}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear contrato: {ex.Message}");
            throw new Exception($"Error al crear contrato en MongoDB: {ex.Message}", ex);
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

    public async Task<Contract> UpdateContract(string id, Contract contract)
    {
        FilterDefinition<Contract> filter = Builders<Contract>.Filter.Eq(c => c.Id, id);
        await _contractCollection.ReplaceOneAsync(filter, contract);
        var updatedContract = await GetContractById(id);
        return updatedContract ?? throw new InvalidOperationException("Error al recuperar el contrato actualizado");
    }

    public async Task<Annex?> GetAnnexById(string contractId, string annexId)
    {
        var contract = await GetContractById(contractId);
        return contract?.Documents.Annexes.FirstOrDefault(a => a.AnnexId == annexId);
    }
}