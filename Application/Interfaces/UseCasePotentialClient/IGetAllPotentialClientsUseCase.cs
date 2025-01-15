using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.PotentialClientDto;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IGetAllPotentialClientsWithProposalsUseCase
{
    Task<List<PotentialClientDto>> Execute(int pageNumber, int pageSize);
}