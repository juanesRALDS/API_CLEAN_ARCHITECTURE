using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories;

public class PotentialClientRepository : IPotentialClientRepository
{
    private readonly IMongoCollection<PotentialClient> _Clientcollection;
    private readonly IMongoCollection<Proposal> _proposalCollection;

    public PotentialClientRepository(MongoDbContext context)
    {
        _Clientcollection = context.GetCollection<PotentialClient>("potentialClients");
        _proposalCollection = context.GetCollection<Proposal>("proposals");
    }

    public async Task<List<PotentialClient>> GetAllPotentialClientsWithProposals(int pageNumber, int pageSize)
    {
        var clients = await _Clientcollection
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        foreach (var client in clients)
        {
            var proposals = await _proposalCollection
                .Find(p => p.PotentialClientId == client.Id)
                .ToListAsync();
            client.Proposals = proposals;
        }

        return clients;
    }

    public async Task CreatePotentialClient(PotentialClient client)
    {
        await _Clientcollection.InsertOneAsync(client);
    }

    public async Task<PotentialClient?> GetByIdPotencialClient(string id)
    {
        return await _Clientcollection.Find(client => client.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdatePotentialClient(string id, PotentialClient client)
    {
        FilterDefinition<PotentialClient>? filter = Builders<PotentialClient>.Filter.Eq(client => client.Id, id);
        UpdateDefinitionBuilder<PotentialClient>? FilterBuilder = Builders<PotentialClient>.Update;
        List<UpdateDefinition<PotentialClient>>? updates = new();

        if (string.IsNullOrEmpty(client.PotentialClientSize))
        {
            updates.Add(FilterBuilder.Set(u => u.PotentialClientSize, client.PotentialClientSize));
        }
        if (string.IsNullOrEmpty(client.CompanyBusinessName))
        {
            updates.Add(FilterBuilder.Set(u => u.CompanyBusinessName, client.CompanyBusinessName));
        }
        if (string.IsNullOrEmpty(client.RepresentativeNames))
        {
            updates.Add(FilterBuilder.Set(u => u.RepresentativeNames, client.RepresentativeNames));
        }
        if (string.IsNullOrEmpty(client.RepresentativeLastNames))
        {
            updates.Add(FilterBuilder.Set(u => u.RepresentativeLastNames, client.RepresentativeLastNames));
        }
        if (string.IsNullOrEmpty(client.ContactPhone))
        {
            updates.Add(FilterBuilder.Set(u => u.ContactPhone, client.ContactPhone));
        }
        if (string.IsNullOrEmpty(client.ContactEmail))
        {
            updates.Add(FilterBuilder.Set(u => u.ContactEmail, client.ContactEmail));
        }
        if (updates.Any())
        {
            await _Clientcollection.UpdateOneAsync(filter, FilterBuilder.Combine(updates));
        }        
    }

    public async Task DeletePoTencialClient(String Id)
    {
        await _Clientcollection.DeleteOneAsync(client => client.Id == Id);
    }
}