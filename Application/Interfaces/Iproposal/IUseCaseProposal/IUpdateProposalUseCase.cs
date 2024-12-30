using SagaAserhi.Application.DTO.ProposalDtos;

namespace SagaAserhi.Application.Interfaces.Iproposal.IUseCaseProposal;

public interface IUpdateProposalUseCase
{
    Task<string> Execute(string id, UpdateProposalDto dto);
}