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
        await _Clientcollection.ReplaceOneAsync(filter, client);
    }

    public async Task DeletePoTencialClient(String Id)
    {
        await _Clientcollection.DeleteOneAsync(client => client.Id == Id);
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