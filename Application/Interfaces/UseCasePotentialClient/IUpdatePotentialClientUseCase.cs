using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient
{
    public interface IUpdatePotentialClientUseCase
    {
        Task<UpdatePotentialClientDto> Execute(string id, UpdatePotentialClientDto dto);
    }
}