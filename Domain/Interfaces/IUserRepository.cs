using api_completa_mongodb_net_6_0.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(int pageNumber, int pageSize);
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User user);
        Task UpdateAsync(string id, User user);
        Task DeleteAsync(string id);
    }
}
