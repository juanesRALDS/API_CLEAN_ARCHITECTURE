using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface ICreatePotentialClientUseCase
{
    Task<string> Execute(CreatePotentialClientDto dto);
}