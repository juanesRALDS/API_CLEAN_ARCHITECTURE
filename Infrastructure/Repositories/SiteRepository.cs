using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories;

public class SiteRepository : ISiteRepository
{
    private readonly IMongoCollection<Site> _sites;

    public SiteRepository(MongoDbContext database)
    {
        _sites = database.GetCollection<Site>("Sites");
    }

    public async Task CreateAsync(Site site)
    {
        await _sites.InsertOneAsync(site);
    }

    public async Task<Site> GetByIdAsync(string id)
    {
        FilterDefinition<Site>? filter = Builders<Site>.Filter.Eq(s => s.Id, id);
        return await _sites.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Site>> GetByProposalIdAsync(string proposalId)
    {
        FilterDefinition<Site>? filter = Builders<Site>.Filter.Eq(s => s.ProposalId, proposalId);
        return await _sites.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(Site site)
    {
        FilterDefinition<Site>? filter = Builders<Site>.Filter.Eq(s => s.Id, site.Id);
        await _sites.ReplaceOneAsync(filter, site);
    }

    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Site>? filter = Builders<Site>.Filter.Eq(s => s.Id, id);
        await _sites.DeleteOneAsync(filter);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        FilterDefinition<Site>? filter = Builders<Site>.Filter.Eq(s => s.Id, id);
        return await _sites.Find(filter).AnyAsync();
    }
}