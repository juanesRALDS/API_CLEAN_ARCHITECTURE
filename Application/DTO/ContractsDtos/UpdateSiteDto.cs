using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using SagaAserhi.Application.DTO.SiteDto;

namespace SagaAserhi.Application.DTO.ContractsDtos;

public class UpdateSiteDto : CreateSiteDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}