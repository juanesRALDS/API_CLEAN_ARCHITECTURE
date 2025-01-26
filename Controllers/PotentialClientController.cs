using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.PotentialClientDto;
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
    private readonly IExcelPotentialClientUseCase _exportExcelUseCase;
    private readonly IExportPotentialClientPdfUseCase _exportPdfUseCase;

    private readonly IExcelPotentialClientUseCase _generateExcelUseCase;

    public PotentialClientController(
        IGetAllPotentialClientsWithProposalsUseCase GetAllPotentialClientsWithProposalsUseCase,
        ICreatePotentialClientUseCase createPotentialClientUseCase,
        IUpdatePotentialClientUseCase updatePotentialClientUseCase,
        IDeletePotentialClientUseCase deletePotentialClientUseCase,
        IExcelPotentialClientUseCase exportExcelUseCase,
        IExportPotentialClientPdfUseCase exportPdfUseCase,
        IExcelPotentialClientUseCase generateExcelUseCase

    )
    {
        _createPotentialClientUseCase = createPotentialClientUseCase;
        _GetAllPotentialClientsWithProposalsUseCase = GetAllPotentialClientsWithProposalsUseCase;
        _updatePotentialClientUseCase = updatePotentialClientUseCase;
        _deletePotentialClientUseCase = deletePotentialClientUseCase;
        _exportExcelUseCase = exportExcelUseCase;
        _exportPdfUseCase = exportPdfUseCase;
        _generateExcelUseCase = generateExcelUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<PotentialClientDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            List<PotentialClientDto>? clients =
                await _GetAllPotentialClientsWithProposalsUseCase.Execute(pageNumber, pageSize);
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
            string? result = await _createPotentialClientUseCase.Execute(dto);
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
            string? result = await _deletePotentialClientUseCase.Execute(Id);
            return Ok(result);
        }
        catch (System.Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("PDF")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExportToExcel(CancellationToken cancellationToken)
    {
        try
        {
            byte[]? fileContent = await _exportPdfUseCase.Execute(cancellationToken);
            return File(
                fileContent,
                "application/pdf",
                $"PotentialClients_{DateTime.Now:yyyyMMdd}.pdf"
            );
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Error al exportar el archivo Excel" });
        }
    }

    [HttpGet("ExportExcel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateExcel(CancellationToken cancellationToken)
    {
        try
        {
            var fileContent = await _generateExcelUseCase.Execute(cancellationToken);
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