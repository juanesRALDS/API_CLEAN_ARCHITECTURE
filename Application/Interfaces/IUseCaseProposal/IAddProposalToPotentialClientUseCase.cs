using SagaAserhi.Application.DTO.ProposalDtos;

namespace SagaAserhi.Application.Interfaces.IUseCaseProposal;

public interface IAddProposalToPotentialClientUseCase
{
    Task<string> Execute(string clientId);
}
