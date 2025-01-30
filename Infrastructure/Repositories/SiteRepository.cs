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
        _sites = database.GetCollection<Site>("site");
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

    public async Task<Site> UpdateSite(string id, Site site)
    {
        FilterDefinition<Site> filter = Builders<Site>.Filter.Eq(s => s.Id, id);
        UpdateDefinition<Site> update = Builders<Site>.Update
            .Set(s => s.Name, site.Name)
            .Set(s => s.Address, site.Address)
            .Set(s => s.City, site.City)
            .Set(s => s.Phone, site.Phone)
            .Set(s => s.ProposalId, site.ProposalId)
            .Set(s => s.Wastes, site.Wastes)
            .Set(s => s.Frequencies, site.Frequencies)
            .Set(s => s.TotalPrice, site.TotalPrice);

        await _sites.UpdateOneAsync(filter, update);
        return await GetByIdAsync(id);
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