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
}