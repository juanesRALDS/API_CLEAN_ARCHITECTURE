using api_completa_mongodb_net_6_0.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftsController : ControllerBase
    {
        private readonly GetAllShiftsUseCase _useCase;

        public ShiftsController(GetAllShiftsUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _useCase.Execute());
    }
}
