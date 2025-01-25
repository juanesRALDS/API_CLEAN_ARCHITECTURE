using SagaAserhi.Application.DTO.ProposalDtos;

namespace SagaAserhi.Application.Interfaces.IUseCaseProposal;

public interface IGetAllProposalsUseCase
{
    Task<(List<ProposalDto> Proposals, int TotalCount)> Execute(int pageNumber, int pageSize, string status = null!);
}