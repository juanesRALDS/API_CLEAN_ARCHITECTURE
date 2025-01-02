using MongoDB.Bson;
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces;
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
                var pipeline = new[]
                {
                    // Match todos los documentos
                    new BsonDocument("$match", new BsonDocument()),

                    // Lookup con potentialClients
                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "potentialClients" },
                        { "localField", "potentialClientId" },
                        { "foreignField", "_id" },
                        { "as", "clientInfo" }
                    }),

                    // Desempaquetar el array de clientInfo
                    new BsonDocument("$unwind", new BsonDocument
                    {
                        { "path", "$clientInfo" },
                        { "preserveNullAndEmptyArrays", true }
                    }),

                    // Agregar el campo companyBusinessName desde clientInfo
                    new BsonDocument("$addFields", new BsonDocument
                    {
                        { "companyBusinessName", "$clientInfo.companyBusinessName" }
                    }),

                    // Paginación
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
            var filter = Builders<Proposal>.Filter.Eq(p => p.Id, id);
            var update = Builders<Proposal>.Update
                .Set(p => p.Title, proposal.Title)
                .Set(p => p.Description, proposal.Description);

            var result = await _proposalCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}