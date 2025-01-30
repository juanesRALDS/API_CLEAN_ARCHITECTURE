using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.UseCases.ContractsUseCase;
using SagaAserhi.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ContractController : ControllerBase
{
    private readonly ICreateContractUseCase _createContractUseCase;
    private readonly IGetAllContractsUseCase _getAllContractsUseCase;

    private readonly IUpdateContractUseCase _updateContractUseCase;
    private readonly IAddAnnexUseCase _addAnnexUseCase;
    private readonly IContractsPDFUseCase _contractsPDFUseCase;


    public ContractController(ICreateContractUseCase createContractUseCase,
            IGetAllContractsUseCase getAllContractsUseCase,
            IUpdateContractUseCase updateContractUseCase,
            IAddAnnexUseCase addAnnexUseCase,
            IContractsPDFUseCase contractsPDFUseCase
            )
    {
        _createContractUseCase = createContractUseCase;
        _getAllContractsUseCase = getAllContractsUseCase;
        _updateContractUseCase = updateContractUseCase;
        _addAnnexUseCase = addAnnexUseCase;
        _contractsPDFUseCase = contractsPDFUseCase;
    }

    [HttpPost("proposals/{proposalId}/contracts")]
    public async Task<ActionResult<Contract>> CreateContract(
     string proposalId,
     [FromForm] string Status,
     [FromForm] DateTime Start,
     [FromForm] DateTime End,
     [FromForm] string ClausesTitle,
     [FromForm] string ClausesContent,
     IFormFileCollection? Files)
    {
        try
        {
            // Crear lista de cláusulas
            var clauses = new List<ClauseDto>
        {
            new ClauseDto
            {
                Title = ClausesTitle ?? "Cláusula General",
                Content = ClausesContent
            }
        };

            // Validar datos mínimos
            if (string.IsNullOrWhiteSpace(ClausesContent))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El contenido de la cláusula es requerido"
                });
            }

            var dto = new CreateContractDto
            {
                Status = Status,
                Start = Start,
                End = End,
                Clauses = clauses,
                Files = Files
            };

            var result = await _createContractUseCase.Execute(proposalId, dto);

            return Created($"/api/contracts/{result.Id}", new
            {
                success = true,
                data = result,
                message = "Contrato creado exitosamente"
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = "Error de validación",
                error = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al crear el contrato",
                error = ex.Message
            });
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<dynamic>> GetAllContracts(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {
            var (contracts, totalCount) = await _getAllContractsUseCase.Execute(pageNumber, pageSize);

            return Ok(new
            {
                success = true,
                data = contracts,
                totalCount,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los contratos",
                error = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContractDto>> UpdateContract(
       string id,
       [FromForm] string Status,
       [FromForm] DateTime Start,
       [FromForm] DateTime End,
       [FromForm] string ClausesTitle,
       [FromForm] string ClausesContent,
       [FromForm] string? AnnexToReplaceId,
       IFormFile? NewFile)
    {
        try
        {
            var clausesList = new List<ClauseDto>
        {
            new() {
                Title = ClausesTitle,
                Content = ClausesContent
            }
        };

            var updateDto = new UpdateContractDto
            {
                Status = Status,
                Start = Start,
                End = End,
                Clauses = clausesList,
                AnnexToReplaceId = AnnexToReplaceId,
                NewFile = NewFile
            };

            ContractDto result = await _updateContractUseCase.Execute(id, updateDto);
            return Ok(new
            {
                success = true,
                data = result,
                message = "Contrato actualizado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }


    [HttpPost("{id}/annexes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AnnexDto>>> AddAnnexes(
        string id,
        [FromForm] IFormFileCollection files)
    {
        try
        {
            var dto = new AddAnnexDto
            {
                ContractId = id,
                Files = files
            };

            var result = await _addAnnexUseCase.Execute(dto);
            return Ok(new
            {
                success = true,
                data = result,
                message = "Anexos agregados exitosamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }



    [HttpGet("generate-pdf/{clientId}/{siteId}")]
    public async Task<IActionResult> GenerateContractPdf(string clientId, string siteId)
    {
        try
        {
            var pdfBytes = await _contractsPDFUseCase.Execute(clientId, siteId);
            return File(pdfBytes, "application/pdf", $"contrato_{clientId}_{siteId}.pdf");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar el contrato: {ex.Message}");
        }
    }
}
