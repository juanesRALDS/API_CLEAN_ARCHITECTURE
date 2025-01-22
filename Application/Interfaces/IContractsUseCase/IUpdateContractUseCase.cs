using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.ContractsDtos;

namespace SagaAserhi.Application.Interfaces.IContractsUseCase;

public interface IUpdateContractUseCase
{
    Task<ContractDto> Execute(string id, UpdateContractDto updateContractDto);
}