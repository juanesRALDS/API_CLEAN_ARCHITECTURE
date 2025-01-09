using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.ISiteUseCase;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/proposals/{proposalId}/sites")]
public class SitesController : ControllerBase
{
    private readonly ICreateSiteUseCase _createSiteUseCase;


    public SitesController(ICreateSiteUseCase createSiteUseCase)
    {
        _createSiteUseCase = createSiteUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<SiteDtos>> CreateSite(
       [FromRoute] string proposalId,
       [FromBody] CreateSiteDto dto)
    {
        try
        {
            dto.ProposalId = proposalId;
            var result = await _createSiteUseCase.Execute(dto); // Solo necesita el dto
            return Created($"/api/proposals/{proposalId}/sites/{result.Id}", result);
        }
        catch (Exception ex) // Agregar catch específico
        {
            return BadRequest(ex.Message);
        }
    }
}