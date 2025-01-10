using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;
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
            FilterDefinition<PotentialClient>? filter = Builders<PotentialClient>.Filter.Empty;
            List<PotentialClient>? clients = await _Clientcollection
               .Find(filter)
               .Skip((pageNumber - 1) * pageSize)
               .Limit(pageSize)
               .ToListAsync();

            if (!clients.Any())
                return new List<PotentialClient>();

            List<string>? proposalIds = clients.SelectMany(c => c.Proposals).ToList();
            FilterDefinition<Proposal>? proposalsFilter = Builders<Proposal>.Filter.In(p => p.Id, proposalIds);

            List<Proposal>? proposals = await _proposalCollection
                .Find(proposalsFilter)
                .ToListAsync();

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


        if (string.IsNullOrEmpty(client.CompanyBusinessName))
        {
            updates.Add(FilterBuilder.Set(u => u.CompanyBusinessName, client.CompanyBusinessName));
        }
        if (string.IsNullOrEmpty(client.ContactPhone))
        {
            updates.Add(FilterBuilder.Set(u => u.ContactPhone, client.ContactPhone));
        }
        if (string.IsNullOrEmpty(client.ContactEmail))
        {
            updates.Add(FilterBuilder.Set(u => u.ContactEmail, client.ContactEmail));
        }
        if (string.IsNullOrEmpty(client.Status))
        {
            updates.Add(FilterBuilder.Set(u => u.Status, client.Status));
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
        try
        {
            // Primero insertamos la propuesta
            await _proposalCollection.InsertOneAsync(proposal);

            // Actualizamos el cliente con el ID de la propuesta
            FilterDefinition<PotentialClient>? filter = Builders<PotentialClient>.Filter.Eq(x => x.Id, clientId);
            UpdateDefinition<PotentialClient>? update = Builders<PotentialClient>.Update.Push(x => x.Proposals, proposal.Id);

            UpdateResult? result = await _Clientcollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al agregar propuesta: {ex.Message}", ex);
        }
    }

    public async Task<List<Proposal>> GetAllProposals(int pageNumber, int pageSize)
    {
        try
        {
            BsonDocument[]? pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument()),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "potentialClients" },
                    { "let", new BsonDocument("localClientId", "$potentialClientId") },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument("$eq", new BsonArray { "$_id", "$$localClientId" }) }
                            })
                        }
                    },
                    { "as", "clientInfo" }
                }),
                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$clientInfo" },
                    { "preserveNullAndEmptyArrays", true }
                }),
                new BsonDocument("$addFields", new BsonDocument
                {
                    { "companyBusinessName", "$clientInfo.companyBusinessName" }
                }),
                new BsonDocument("$skip", (pageNumber - 1) * pageSize),
                new BsonDocument("$limit", pageSize)
            };

            return await _proposalCollection
                .Aggregate<Proposal>(pipeline)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener propuestas: {ex.Message}", ex);
        }
    }
    public async Task<IEnumerable<PotentialClient>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _Clientcollection.Find(_ => true)
                              .ToListAsync(cancellationToken);
    }
}