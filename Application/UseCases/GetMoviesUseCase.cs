using System.Collections.Generic;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;

namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class GetMoviesUseCase
    {
        private readonly IMovieRepository _repository;

        public GetMoviesUseCase(IMovieRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<Movie>, int)> ExecuteAsync(int page, int pageSize, string genre = null)
        {
            return await _repository.GetMoviesAsync(page, pageSize, genre);
        }
    }
}
