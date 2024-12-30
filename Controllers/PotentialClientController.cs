using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PotentialClientController : ControllerBase
{
    private readonly IGetAllPotentialClientsWithProposalsUseCase _GetAllPotentialClientsWithProposalsUseCase;
    private readonly ICreatePotentialClientUseCase _createPotentialClientUseCase;
    private readonly IUpdatePotentialClientUseCase _updatePotentialClientUseCase;
    private readonly IDeletePotentialClientUseCase _deletePotentialClientUseCase;

    public PotentialClientController(
        IGetAllPotentialClientsWithProposalsUseCase GetAllPotentialClientsWithProposalsUseCase,
        ICreatePotentialClientUseCase createPotentialClientUseCase,
        IUpdatePotentialClientUseCase updatePotentialClientUseCase,
        IDeletePotentialClientUseCase deletePotentialClientUseCase

    )
    {
        _createPotentialClientUseCase = createPotentialClientUseCase;
        _GetAllPotentialClientsWithProposalsUseCase = GetAllPotentialClientsWithProposalsUseCase;
        _updatePotentialClientUseCase = updatePotentialClientUseCase;
        _deletePotentialClientUseCase = deletePotentialClientUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<PotentialClientDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var clients = await _GetAllPotentialClientsWithProposalsUseCase.Execute(pageNumber, pageSize);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdatePotentialClientDto dto)
    {
        try
        {
            UpdatePotentialClientDto? result = await _updatePotentialClientUseCase.Execute(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(String Id)
    {
        try
        {
            var result = await _deletePotentialClientUseCase.Execute(Id);
            return Ok(result);
        }
        catch (System.Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }


}