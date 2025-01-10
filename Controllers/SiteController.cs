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
    private readonly IGetSiteUseCase _getSiteUseCase;
    private readonly IGetSiteUseCase _GetSiteUseCase;

    public SitesController(ICreateSiteUseCase createSiteUseCase,
                            IGetSiteUseCase getSiteUseCase,
                            IGetSiteUseCase GetSiteUseCase
    )
    {
        _createSiteUseCase = createSiteUseCase;
        _getSiteUseCase = getSiteUseCase;
        _GetSiteUseCase = GetSiteUseCase;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SiteDtos>> CreateSite([FromBody] CreateSiteDto dto)
    {
        try
        {
            string? proposalId = (string)RouteData.Values["proposalId"]!;
            SiteRequestDto? siteRequest = new SiteRequestDto
            {
                ProposalId = proposalId,
                SiteInfo = dto
            };

            SiteDtos? result = await _createSiteUseCase.Execute(siteRequest);
            return Created($"/api/proposals/{proposalId}/sites/{result.Id}", result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }



    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<SiteDtos>>> GetAllSites(string proposalId)
    {
        try
        {
            List<SiteDtos>? sites = await _GetSiteUseCase.Execute(proposalId);
            return Ok(sites);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}