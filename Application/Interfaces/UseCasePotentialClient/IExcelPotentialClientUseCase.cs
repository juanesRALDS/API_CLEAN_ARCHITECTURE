using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IExcelPotentialClientUseCase
{
    Task<byte[]> Execute(CancellationToken cancellationToken);
}