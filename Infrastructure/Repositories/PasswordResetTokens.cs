using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IMongoCollection<PasswordResetToken> _collection;

        public PasswordResetTokenRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PasswordResetToken>("PasswordResetTokens");
        }

        public async Task InsertAsync(PasswordResetToken token)
        {
            await _collection.InsertOneAsync(token);
        }
    }
}
