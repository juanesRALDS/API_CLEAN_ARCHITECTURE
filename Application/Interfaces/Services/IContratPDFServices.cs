using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.Services;

public interface IContratPDFServices
{
    Task<byte[]> GenerateContractPDFServices(PotentialClient client, Site site);
}