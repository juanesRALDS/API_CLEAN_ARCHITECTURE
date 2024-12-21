using MongoDB.Driver;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories;

public class PotentialClientRepository : IPotentialClientRepository
{
    private readonly IMongoCollection<PotentialClient> _collection;

    public PotentialClientRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<PotentialClient>("potentialClients");
    }

    public async Task<List<PotentialClient>> GetAllPotentialClients(int pageNumber, int pageSize)
    {
        return await _collection
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task CreatePotentialClient(PotentialClient client)
    {
        await _collection.InsertOneAsync(client);
    }

    public async Task<PotentialClient?> GetByIdPotencialClient(string id)
    {
        return await _collection.Find(client => client.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdatePotentialClient(string id, PotentialClient client)
    {
        await _collection.ReplaceOneAsync(client => client.Id == id, client);
    }

    public async Task DeletePoTencialClient(String Id)
    {
        await _collection.DeleteOneAsync(client => client.Id == Id);
    }
}