using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly IMongoCollection<Proposal> _proposalCollection;

        public ProposalRepository(MongoDbContext context)
        {
            _proposalCollection = context.GetCollection<Proposal>("proposals");
        }

        public async Task<(List<Proposal> Proposals, int TotalCount)> GetAllProposals(int pageNumber, int pageSize)
        {
            try
            {
                var countFacet = AggregateFacet.Create("count",
                    PipelineDefinition<Proposal, AggregateCountResult>.Create(new[]
                    {
                        PipelineStageDefinitionBuilder.Count<Proposal>()
                    }));

                var dataFacet = AggregateFacet.Create("data",
                    PipelineDefinition<Proposal, Proposal>.Create(new[]
                    {
                        PipelineStageDefinitionBuilder.Skip<Proposal>((pageNumber - 1) * pageSize),
                        PipelineStageDefinitionBuilder.Limit<Proposal>(pageSize)
                    }));

                var pipeline = new[]
                {
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "potentialClients" },
                        { "localField", "clientId" },
                        { "foreignField", "_id" },
                        { "as", "client" }
                    }),
                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$client" },
                        { "preserveNullAndEmptyArrays", true }
                    }),
                    new BsonDocument("$project", new BsonDocument
                    {
                        { "_id", 1 },
                        { "clientId", 1 },
                        { "number", 1 },
                        { "status", 1 },
                        { "sites", 1 },
                        { "history", 1 },
                        { "createdAt", 1 },
                        { "updatedAt", 1 },
                        { "companyBusinessName", "$client.businessInfo.businessName" }
                    })
                };

                var aggregation = await _proposalCollection
                    .Aggregate<Proposal>(pipeline)
                    .ToListAsync();

                var totalCount = await _proposalCollection.CountDocumentsAsync(new BsonDocument());

                return (aggregation, (int)totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener propuestas: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateProposal(Proposal proposal)
        {
            try
            {
                await _proposalCollection.InsertOneAsync(proposal);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Proposal> GetProposalById(string id)
        {
            return await _proposalCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProposal(string id, Proposal proposal)
        {
            try
            {
                var filter = Builders<Proposal>.Filter.Eq(p => p.Id, id);
                var update = Builders<Proposal>.Update
                    .Set(p => p.Status.Proposal, proposal.Status.Proposal)
                    .Set(p => p.Status.Sending, proposal.Status.Sending)
                    .Set(p => p.Status.Review, proposal.Status.Review)
                    .Set(p => p.UpdatedAt, proposal.UpdatedAt);

                var result = await _proposalCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _proposalCollection.Find(_ => true)
                                          .ToListAsync(cancellationToken);
        }


        public async Task<bool> HasExistingSite(string proposalId)
        {
            var proposal = await GetProposalById(proposalId);
            return proposal?.Sites.Any() ?? false;
        }

        public async Task<bool> UpdateProposalSite(string proposalId, string siteId)
        {
            try
            {
                var filter = Builders<Proposal>.Filter.Eq(p => p.Id, proposalId);
                var update = Builders<Proposal>.Update.Push(p => p.Sites, new Site { Id = siteId });

                var result = await _proposalCollection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}