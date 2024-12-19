using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PotentialClientController : ControllerBase
{
    private readonly IGetAllPotentialClientsUseCase _getAllPotentialClientsUseCase;

    public PotentialClientController(IGetAllPotentialClientsUseCase getAllPotentialClientsUseCase)
    {
        _getAllPotentialClientsUseCase = getAllPotentialClientsUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<PotentialClientDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var clients = await _getAllPotentialClientsUseCase.Execute(pageNumber, pageSize);
            return Ok(clients);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}