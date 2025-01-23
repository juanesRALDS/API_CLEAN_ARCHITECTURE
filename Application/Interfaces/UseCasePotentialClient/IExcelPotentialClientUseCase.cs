using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IExcelPotentialClientUseCase
{
    Task<ExcelfileClientDto> Execute(int pageNumber, int pageSize, CancellationToken cancellationToken);
}