using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IGetAllPotentialClientsUseCase
{
    Task<List<PotentialClientDto>> Execute(int pageNumber, int pageSize);
}