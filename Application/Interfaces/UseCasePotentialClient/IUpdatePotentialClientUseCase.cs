using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient
{
    public interface IUpdatePotentialClientUseCase
    {
        Task<string> Execute(string id, UpdatePotentialClientDto dto);
    }
}