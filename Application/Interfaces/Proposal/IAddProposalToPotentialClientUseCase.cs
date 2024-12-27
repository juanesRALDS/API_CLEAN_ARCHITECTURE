using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient
{
    public interface IAddProposalToPotentialClientUseCase
    {
        Task<string> Execute(string clientId, CreateProposalDto proposalDto);
    }
}