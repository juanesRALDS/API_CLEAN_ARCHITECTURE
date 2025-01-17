using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IGetAllProposalsUseCase _getAllProposalsUseCase;
    private readonly IAddProposalToPotentialClientUseCase _addProposalToPotentialClientUseCase;
    private readonly IUpdateProposalUseCase _updateProposalUseCase;
    private readonly IExcelProposalUseCase _exportProposalUseCase;

    public ProposalController(
        IGetAllProposalsUseCase getAllProposalsUseCase,
        IAddProposalToPotentialClientUseCase addProposalToPotentialClientUseCase,
        IUpdateProposalUseCase updateProposalUseCase,
        IExcelProposalUseCase exportProposalUseCase
    )
    {
        _getAllProposalsUseCase = getAllProposalsUseCase;
        _addProposalToPotentialClientUseCase = addProposalToPotentialClientUseCase;
        _updateProposalUseCase = updateProposalUseCase;
        _exportProposalUseCase = exportProposalUseCase;

    }

    [HttpGet]
    public async Task<ActionResult<List<ProposalDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            List<ProposalDto>? result =
                await _getAllProposalsUseCase.Execute(pageNumber, pageSize);
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

    [HttpPost("clients/{clientId}/proposals")]
    public async Task<IActionResult> CreateProposal(string clientId, [FromBody] CreateProposalDto dto)
    {
        try
        {
            var result = await _addProposalToPotentialClientUseCase.Execute(clientId, dto);
            return Ok(new { Message = result });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Error = "Error interno del servidor" });
        }
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProposalDto dto)
    {
        try
        {
            string? result = await _updateProposalUseCase.Execute(id, dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Agregar en el controlador existente
    [HttpGet("export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExportToExcel(CancellationToken cancellationToken)
    {
        try
        {
            byte[]? fileContent = await _exportProposalUseCase.ExecuteAsync(cancellationToken);
            return File(
                fileContent,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Proposals_{DateTime.Now:yyyyMMdd}.xlsx"
            );
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Error al exportar el archivo Excel" });
        }
    }
}