using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.ISiteUseCase;

namespace SagaAserhi.Controllers;

[ApiController]
[Route("api/proposals/{proposalId}/sites")]
public class SitesController : ControllerBase
{
    private readonly ICreateSiteUseCase _createSiteUseCase;
    private readonly IGetSiteUseCase _getSiteUseCase;
    private readonly IGetSiteUseCase _GetSiteUseCase;
    private readonly IUpdateSiteUseCase _updateSiteUseCase;

    public SitesController(ICreateSiteUseCase createSiteUseCase,
                            IGetSiteUseCase getSiteUseCase,
                            IGetSiteUseCase GetSiteUseCase,
                            IUpdateSiteUseCase updateSiteUseCase
    )
    {
        _createSiteUseCase = createSiteUseCase;
        _getSiteUseCase = getSiteUseCase;
        _GetSiteUseCase = GetSiteUseCase;
        _updateSiteUseCase = updateSiteUseCase;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SiteDtos>> CreateSite([FromBody] CreateSiteDto dto)
    {
        try
        {
            string proposalId = (string)RouteData.Values["proposalId"]!;
            var siteRequest = new SiteRequestDto
            {
                ProposalId = proposalId,
                SiteInfo = dto
            };

            var result = await _createSiteUseCase.Execute(siteRequest);

            return Created($"/api/proposals/{proposalId}/sites/{result.Id}", new
            {
                success = true,
                data = result,
                message = "Sitio creado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al crear el sitio",
                error = ex.Message
            });
        }
    }



    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<SiteDtos>>> GetAllSites(string proposalId)
    {
        try
        {
            List<SiteDtos>? sites = await _getSiteUseCase.Execute(proposalId);
            return Ok(sites);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateSiteDto>> UpdateSite(string id, [FromBody] UpdateSiteDto updateSiteDto)
    {
        try
        {
            UpdateSiteDto result = await _updateSiteUseCase.Execute(id, updateSiteDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
