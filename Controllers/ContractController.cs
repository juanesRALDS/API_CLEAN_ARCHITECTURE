using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ContractController : ControllerBase
{
    private readonly ICreateContractUseCase _createContractUseCase;
    private readonly IGetAllContractsUseCase _getAllContractsUseCase;

    public ContractController(ICreateContractUseCase createContractUseCase,
            IGetAllContractsUseCase getAllContractsUseCase)
    {
        _createContractUseCase = createContractUseCase;
        _getAllContractsUseCase = getAllContractsUseCase;
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


}