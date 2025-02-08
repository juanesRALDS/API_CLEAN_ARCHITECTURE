using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.SiteDto;

namespace SagaAserhi.Application.Interfaces.ISiteUseCase;

public interface IGetSiteUseCase
{
    Task<List<SiteDtos>> Execute(string proposalId, int pageNumber, int pageSize);
}