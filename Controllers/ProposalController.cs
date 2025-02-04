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
    public async Task<ActionResult<dynamic>> GetAll(
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? status = null)
    {
        try
        {
            (List<ProposalDto> proposals, int totalCount) = await _getAllProposalsUseCase.Execute(pageNumber, pageSize, status!);

            return Ok(new
            {
                Success = true,
                Data = proposals,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = status,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Success = false, Message = "Error interno del servidor", Error = ex.Message });
        }
    }

    [HttpPost("clients/{clientId}/proposals")]
    public async Task<IActionResult> CreateProposal(string clientId)
    {
        try
        {
            string? result = await _addProposalToPotentialClientUseCase.Execute(clientId);
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
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Error interno del servidor", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProposalDto dto)
    {
        try
        {
            string? result = await _updateProposalUseCase.Execute(id, dto);
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
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Error interno del servidor", error = ex.Message });
        }
    }

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
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al exportar el archivo Excel", error = ex.Message });
        }
    }
}