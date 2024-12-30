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
        try
        {
            var filter = Builders<PotentialClient>.Filter.Empty;
            var clients = await _Clientcollection
                .Find(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            if (!clients.Any())
                return new List<PotentialClient>();

            var clientIds = clients.Select(c => c.Id).ToList();
            var proposalsFilter = Builders<Proposal>.Filter.In(p => p.PotentialClientId, clientIds);

            var proposals = await _proposalCollection
                .Find(proposalsFilter)
                .ToListAsync();

            var proposalsByClientId = proposals
                .GroupBy(p => p.PotentialClientId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var client in clients)
            {
                client.Proposals = proposalsByClientId.TryGetValue(client.Id, out var clientProposals)
                    ? clientProposals
                    : new List<Proposal>();
            }

            return clients;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener clientes con propuestas: {ex.Message}", ex);
        }
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

        if (string.IsNullOrEmpty(client.PersonType))
        {
            updates.Add(FilterBuilder.Set(u => u.PersonType, client.PersonType));
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

    public async Task<bool> AddProposalToPotentialClient(string clientId, Proposal proposal)
    {
        await _proposalCollection.InsertOneAsync(proposal);
        FilterDefinition<PotentialClient>? filter = Builders<PotentialClient>.Filter.Eq(x => x.Id, clientId);
         UpdateDefinition<PotentialClient>? update = Builders<PotentialClient>.Update.Push(x => x.Proposals, proposal);

        UpdateResult? result = await _Clientcollection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }
}