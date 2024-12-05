using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _moviesCollection;

        public MovieRepository(MongoDbContext context)
        {
            _moviesCollection = context.GetCollection<Movie>("movies");
        }

        public async Task<(IEnumerable<Movie>, int)> GetMoviesAsync(int page, int pageSize, string genreFilter = null)
        {
            FilterDefinition<Movie>? filter = string.IsNullOrEmpty(genreFilter)
                ? Builders<Movie>.Filter.Empty
                : Builders<Movie>.Filter.AnyEq(m => m.Genres, genreFilter);

            long total = await _moviesCollection.CountDocumentsAsync(filter);
            List<Movie>? movies = await _moviesCollection
                .Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (movies, (int)total);
        }
    }
}
