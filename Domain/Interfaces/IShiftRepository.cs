

using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IShiftRepository
    {
        Task<List<Shift>> GetAllAsync();
        Task<Shift?> GetByIdAsync(string id);
        Task AddAsync(Shift shift);
    }
}
