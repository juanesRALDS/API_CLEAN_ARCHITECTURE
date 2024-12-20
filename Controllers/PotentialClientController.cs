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
    private readonly ICreatePotentialClientUseCase _createPotentialClientUseCase;

    public PotentialClientController(
        IGetAllPotentialClientsUseCase getAllPotentialClientsUseCase,
        ICreatePotentialClientUseCase createPotentialClientUseCase
    )
    {
        _createPotentialClientUseCase = createPotentialClientUseCase;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePotentialClientDto dto)
    {
        try
        {
            var result = await _createPotentialClientUseCase.Execute(dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}