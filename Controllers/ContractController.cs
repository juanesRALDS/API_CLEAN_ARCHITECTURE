// Controllers/ContractController.cs
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.Interfaces.IContractsUseCase;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractController : ControllerBase
{
    private readonly IGetAllContractsUseCase _getAllContractsUseCase;

    public ContractController(IGetAllContractsUseCase getAllContractsUseCase)
    {
        _getAllContractsUseCase = getAllContractsUseCase;
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
}