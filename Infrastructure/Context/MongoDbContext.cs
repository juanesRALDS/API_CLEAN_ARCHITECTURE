using SagaAserhi.Infrastructure.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SagaAserhi.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> settings)
        {
            if (string.IsNullOrEmpty(settings.Value.ConnectionString))
                throw new ArgumentNullException(nameof(settings.Value.ConnectionString), "La cadena de conexión no puede ser nula");

            MongoClient? client = new(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) =>
            _database.GetCollection<T>(collectionName);
    }
}
