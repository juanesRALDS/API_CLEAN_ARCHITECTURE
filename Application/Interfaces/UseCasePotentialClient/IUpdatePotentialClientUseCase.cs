using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.PotentialClientDto;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IUpdatePotentialClientUseCase
{
    Task<UpdatePotentialClientDto> Execute(string id, UpdatePotentialClientDto dto);
}