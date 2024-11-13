using MongoDB.Driver;
using Microsoft.Extensions.Options;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Models;

namespace api_completa_mongodb_net_6_0.Services
{
    public class UserServices
    {
        private readonly IMongoCollection<User> _collection;

        public UserServices(IOptions<MongoDBSettings> mongoDBSettings)
        {
            if (mongoDBSettings?.Value == null || string.IsNullOrEmpty(mongoDBSettings.Value.ConnectionString))
            {
                throw new ArgumentNullException("La cadena de conexión no puede ser nula");
            }

            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _collection = database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _collection.InsertOneAsync(user);

        public async Task UpdateAsync(string id, User updateUser) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updateUser);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
