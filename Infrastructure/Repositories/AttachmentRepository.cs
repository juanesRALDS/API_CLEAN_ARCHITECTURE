using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;

namespace SagaAserhi.Infrastructure.Repositories
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly IMongoCollection<Attachment> _collection;

        public AttachmentRepository(MongoDbContext database)
        {
            _collection = database.GetCollection<Attachment>("attachments");
        }

        public async Task<Attachment> UploadAsync(Attachment attachment, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(attachment, new InsertOneOptions(), cancellationToken);
            return attachment;
        }

        public async Task<Attachment> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<Attachment>.Filter.Eq(x => x.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Attachment>> GetByClientIdAsync(string clientId, CancellationToken cancellationToken)
        {
            var filter = Builders<Attachment>.Filter.Eq(x => x.PotentialClientId, clientId);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<Attachment>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter, cancellationToken);
            return result.DeletedCount > 0;
        }
    }
}