using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Context;
using MongoDB.Driver;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _UserCollection;

    public UserRepository(MongoDbContext context)
    {
        _UserCollection = context.GetCollection<User>("users");
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("mail cannot be null or emply", nameof(email));
        }
        return await _UserCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAllUser(int pageNumber, int pageSize)
    {
        return await _UserCollection
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task<User?> GetUserById(string id)
    {
        return await _UserCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateNewUser(User user) =>
        await _UserCollection.InsertOneAsync(user);

    public async Task UpdateUser(string id, User user)
    {
        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(u => u.Id, id);
        UpdateDefinitionBuilder<User>? updateBuilder = Builders<User>.Update;
        List<UpdateDefinition<User>>? updates = new List<UpdateDefinition<User>>();

        if (!string.IsNullOrEmpty(user.Name))
            updates.Add(updateBuilder.Set(u => u.Name, user.Name));

        if (!string.IsNullOrEmpty(user.Email))
            updates.Add(updateBuilder.Set(u => u.Email, user.Email));

        if (updates.Any())
            await _UserCollection.UpdateOneAsync(filter, updateBuilder.Combine(updates));
    }

    public async Task DeleteUser(string id) =>
        await _UserCollection.DeleteOneAsync(user => user.Id == id);

    public async Task UpdatePassword(string userId, string hashedPassword)
    {
        FilterDefinition<User>? filter = Builders<User>.Filter.Eq(u => u.Id, userId);
        UpdateDefinition<User>? update = Builders<User>.Update.Set(u => u.Password, hashedPassword);

        await _UserCollection.UpdateOneAsync(filter, update);
    }
}
