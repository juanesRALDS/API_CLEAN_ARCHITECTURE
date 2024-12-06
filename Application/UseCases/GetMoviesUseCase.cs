using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;
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

        public async Task<(IEnumerable<MovieDto>, int)> Login(int page, int pageSize, string genre = null)
        {
            // Obtener las películas y el total desde el repositorio
            (IEnumerable<Movie> movies, int total) = await _repository.GetMoviesAsync(page, pageSize, genre);

            //mapeo
            IEnumerable<MovieDto> movieDtos = movies.Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genres = movie.Genres,
                Runtime = movie.Runtime,
                Plot = movie.Plot,
                Cast = movie.Cast,
                Directors = movie.Directors,
                Year = movie.Year,
                Imdb = new ImdbDto
                {
                    Rating = movie.Imdb.Rating,
                    Votes = movie.Imdb.Votes,
                    Id = movie.Imdb.Id
                }
            });

            return (movieDtos, total);
        }
    }
}
