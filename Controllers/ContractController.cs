// Controllers/ContractController.cs
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractController : ControllerBase
{
    private readonly IGetAllContractsUseCase _getAllContractsUseCase;
    private readonly ICreateContractUseCase _createContractUseCase;

    public ContractController(IGetAllContractsUseCase getAllContractsUseCase,
            ICreateContractUseCase createContractUseCase)
    {
        _getAllContractsUseCase = getAllContractsUseCase;
        _createContractUseCase = createContractUseCase;

    }

    [HttpGet]
    public async Task<ActionResult<dynamic>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var (contracts, totalCount) = await _getAllContractsUseCase.Execute(pageNumber, pageSize);

            return Ok(new
            {
                Success = true,
                Data = contracts,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
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

    [HttpPost("proposals/{proposalId}/contracts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Contract>> CreateContract(
        string proposalId,
        [FromForm] CreateContractDto dto)
    {
        try
        {
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
}