using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IGetAllPotentialClientsWithProposalsUseCase
{
    Task<List<PotentialClientDto>> Execute(int pageNumber, int pageSize);
}