using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
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
