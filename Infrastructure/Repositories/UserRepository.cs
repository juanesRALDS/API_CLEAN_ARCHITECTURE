using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("users");
        }

        public async Task CreateAsync(User user) => await _collection.InsertOneAsync(user);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }
}