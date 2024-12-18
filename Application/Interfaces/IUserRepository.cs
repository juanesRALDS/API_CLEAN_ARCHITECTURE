using SagaAserhi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser(int pageNumber, int pageSize);
        Task<User?> GetUserById(string id);
        Task<User?> GetUserByEmail(string email);
        Task CreateNewUser(User user);
        Task UpdateUser(string id, User user);
        Task DeleteUser(string id);
        Task UpdatePassword(string userId, string hashedPassword);
    }
}
