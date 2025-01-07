using SagaAserhi.Application.DTO.ProposalDtos;

namespace SagaAserhi.Application.Interfaces.IUseCaseProposal;

public interface IGetAllProposalsUseCase
{
    Task<List<ProposalDto>> Execute(int pageNumber, int pageSize);
}