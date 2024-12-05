
using System.Collections.Generic;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Domain.Entities;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<(IEnumerable<Movie>, int)> GetMoviesAsync(int page, int pageSize, string genreFilter);
    }
}
