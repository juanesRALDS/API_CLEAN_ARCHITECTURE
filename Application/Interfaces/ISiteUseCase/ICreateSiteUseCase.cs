using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.SiteDto;

namespace SagaAserhi.Application.Interfaces.ISiteUseCase
{
    public interface ICreateSiteUseCase
    {
        Task<SiteDtos> Execute(CreateSiteDto dto);
    }
}