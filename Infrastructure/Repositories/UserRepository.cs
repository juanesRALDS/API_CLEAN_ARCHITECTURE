using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoCollection<User> collection)
        {
            _collection = collection;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _collection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _collection
                .Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id) =>
            await _collection.Find(user => user.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _collection.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User user)
        {
            FilterDefinition<User>? filter = Builders<User>.Filter.Eq(u => u.Id, id);
             UpdateDefinition<User>? update = Builders<User>.Update
                .Set(u => u.Name, user.Name)
                .Set(u => u.Email, user.Email)
                .Set(u => u.Password, user.Password);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(user => user.Id == id);
    }
}
