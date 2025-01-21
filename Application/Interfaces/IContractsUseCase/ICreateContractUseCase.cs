using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IContractsUseCase;

public interface ICreateContractUseCase
{
    Task<Contract> Execute(string proposalId, CreateContractDto dto);
}