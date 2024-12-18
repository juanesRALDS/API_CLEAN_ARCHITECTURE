using SagaAserhi.Domain.Entities;
using SagaAserhi.Domain.Interfaces.Auth;
using SagaAserhi.Infrastructure.Context;
using MongoDB.Driver;

namespace SagaAserhi.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IMongoCollection<Token> _tokensCollection;

        public PasswordResetTokenRepository(MongoDbContext database)
        {
            _tokensCollection = database.GetCollection<Token>("Token");
        }

        public async Task SaveToken(Token tokens)
        {
            await _tokensCollection.InsertOneAsync(tokens);
        }

        public async Task<Token> GetByToken(string Tokens)
        {
            return await _tokensCollection
                .Find(t => t.Tokens == Tokens)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteToken(string tokenValue)
        {
            await _tokensCollection.DeleteOneAsync(t => t.Tokens == tokenValue);
        }
    }
}
