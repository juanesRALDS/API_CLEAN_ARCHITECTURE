using MongoDB.Bson;
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;
using ZstdSharp.Unsafe;

namespace SagaAserhi.Infrastructure.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly IMongoCollection<Proposal> _proposalCollection;

        public ProposalRepository(MongoDbContext context)
        {
            _proposalCollection = context.GetCollection<Proposal>("proposals");
        }


        public async Task<List<Proposal>> GetAllProposals(int pageNumber, int pageSize)
        {
            try
            {
                BsonDocument[]? pipeline = new[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "potentialClients" },
                        { "let", new BsonDocument("clientId", "$potentialClientId") },
                        { "pipeline", new BsonArray
                            {
                                new BsonDocument("$match", new BsonDocument
                                {
                                    { "$expr", new BsonDocument("$eq", new BsonArray
                                        {
                                            "$_id",
                                            new BsonDocument("$toObjectId", "$$clientId")
                                        })
                                    }
                                }),
                                new BsonDocument("$project", new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "companyBusinessName", 1 }
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
                    new BsonDocument("$set", new BsonDocument
                    {
                        { "companyBusinessName", "$clientInfo.companyBusinessName" },
                        { "potentialClientId", "$clientInfo._id" }
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


        public Task<Proposal> GetProposalById(string id)
        {
            return _proposalCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProposal(string id, Proposal proposal)
        {
            FilterDefinition<Proposal>? filter = Builders<Proposal>.Filter.Eq(p => p.Id, id);
            UpdateDefinition<Proposal>? update = Builders<Proposal>.Update
                .Set(p => p.Title, proposal.Title)
                .Set(p => p.Description, proposal.Description);

            UpdateResult? result = await _proposalCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _proposalCollection.Find(_ => true)
                                  .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateProposalSite(string proposalId, string siteId)
        {
            FilterDefinition<Proposal>? filter = Builders<Proposal>.Filter.Eq(p => p.Id, proposalId);
            UpdateDefinition<Proposal>? update = Builders<Proposal>.Update
                .Set(p => p.SiteId, siteId)
                .Set(p => p.HasSite, true)
                .Set(p => p.LastModified, DateTime.UtcNow);

            UpdateResult? result = await _proposalCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> HasExistingSite(string proposalId)
        {
            Proposal? proposal = await GetProposalById(proposalId);
            return proposal?.HasSite ?? false;
        }
    }
}