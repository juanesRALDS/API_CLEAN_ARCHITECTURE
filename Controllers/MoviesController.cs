using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly GetMoviesUseCase _getMoviesUseCase;

        public MoviesController(GetMoviesUseCase getMoviesUseCase)
        {
            _getMoviesUseCase = getMoviesUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? genre = null)
        {
            (IEnumerable<MovieDto> movies, int total)
                = await _getMoviesUseCase.Login(page, pageSize, genre);
                return Ok(new { Data = movies, Total = total });
        }
    }
}
