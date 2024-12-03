using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IMongoCollection<Token> _tokensCollection;

        public PasswordResetTokenRepository(MongoDbContext database)
        {
            // Nombre de la colección donde se guardarán los tokens
            _tokensCollection = database.GetCollection<Token>("Token");
        }

        public async Task SaveTokenAsync(Token tokens)
        {
            // Inserta el token en la colección
            await _tokensCollection.InsertOneAsync(tokens);
        }

        public async Task<Token> GetByTokenAsync(string Tokens)
        {
            // Busca el token en la colección
            return await _tokensCollection
                .Find(t => t.Tokens == Tokens)
                .FirstOrDefaultAsync();

            
        }

        public async Task DeleteTokenAsync(string tokenValue)
        {
            // Elimina el token en la colección
            await _tokensCollection.DeleteOneAsync(t => t.Tokens == tokenValue);
        }
    }
}
