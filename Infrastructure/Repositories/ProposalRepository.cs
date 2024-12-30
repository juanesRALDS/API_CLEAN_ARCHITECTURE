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
            return await _proposalCollection
                .Find(Builders<Proposal>.Filter.Empty)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
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