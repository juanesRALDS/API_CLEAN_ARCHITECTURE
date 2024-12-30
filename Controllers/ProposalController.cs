using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.Iproposal.IUseCaseProposal;
using SagaAserhi.Application.Interfaces.Proposal.UseCaseProposal;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProposalController : ControllerBase
{
    private readonly IGetAllProposalsUseCase _getAllProposalsUseCase;
    private readonly IAddProposalToPotentialClientUseCase _addProposalToPotentialClientUseCase;
    private readonly IUpdateProposalUseCase _updateProposalUseCase;

    public ProposalController(
        IGetAllProposalsUseCase getAllProposalsUseCase,
        IAddProposalToPotentialClientUseCase addProposalToPotentialClientUseCase,
        IUpdateProposalUseCase updateProposalUseCase
    )
    {
        _getAllProposalsUseCase = getAllProposalsUseCase;
        _addProposalToPotentialClientUseCase = addProposalToPotentialClientUseCase;
        _updateProposalUseCase = updateProposalUseCase;
        
    }

    [HttpGet]
    public async Task<ActionResult<List<ProposalDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var result = await _getAllProposalsUseCase.Execute(pageNumber, pageSize);
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

    [HttpPost("{id}/proposals")]
    public async Task<IActionResult> AddProposal(string id, [FromBody] CreateProposalDto dto)
    {
        try
        {
            var result = await _addProposalToPotentialClientUseCase.Execute(id, dto);
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
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProposalDto dto)
    {
        try
        {
            var result = await _updateProposalUseCase.Execute(id, dto);
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
}