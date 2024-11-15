
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using MongoDB.Driver;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly IMongoCollection<Shift> _collection;

        public ShiftRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<Shift>("shifts");
        }

        public async Task<List<Shift>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

        public async Task<Shift?> GetByIdAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Shift shift) => await _collection.InsertOneAsync(shift);
    }
}
