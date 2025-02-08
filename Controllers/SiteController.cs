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
[Route("api/sites")]
public class SitesController : ControllerBase
{
    private readonly ICreateSiteUseCase _createSiteUseCase;
    private readonly IGetSiteUseCase _getSiteUseCase;
    private readonly IUpdateSiteUseCase _updateSiteUseCase;
    private readonly IDeleteSiteUseCase _deleteSiteUseCase;

    public SitesController(ICreateSiteUseCase createSiteUseCase,
                            IGetSiteUseCase getSiteUseCase,
                            IUpdateSiteUseCase updateSiteUseCase,
                            IDeleteSiteUseCase deleteSiteUseCase
    )
    {
        _createSiteUseCase = createSiteUseCase;
        _getSiteUseCase = getSiteUseCase;
        _updateSiteUseCase = updateSiteUseCase;
        _deleteSiteUseCase = deleteSiteUseCase;
    }

    [HttpPost("createSite{ProposalID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SiteDtos>> CreateSite([FromBody] CreateSiteDto dto)
    {
        try
        {
            string proposalId = (string)RouteData.Values["proposalId"]!;
            SiteRequestDto? siteRequest = new()
            {
                ProposalId = proposalId,
                SiteInfo = dto
            };

            SiteDtos? result = await _createSiteUseCase.Execute(siteRequest);

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



    [HttpGet("getSite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<SiteDtos>>> GetAllSites(string proposalId, 
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
     )
    {
        try
        {
            List<SiteDtos>? sites = await _getSiteUseCase.Execute(proposalId, pageNumber, pageSize);
            return Ok(sites);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("putSite{SiteID}")]
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

    [HttpDelete("deleteSite{SiteID}")]

    public async Task<IActionResult> DeleteSite(string id)
    {
        try
        {
            await _deleteSiteUseCase.Execute(id);
            return Ok(new
            {
                success = true,
                message = "Sitio eliminado exitosamente"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al eliminar el sitio",
                error = ex.Message
            });
        }
    }
}
