using SagaAserhi.Application.DTO.ProposalDtos;

namespace SagaAserhi.Application.Interfaces.Proposal.UseCaseProposal
{
    public interface IGetAllProposalsUseCase
    {
        Task<List<ProposalDto>> Execute(int pageNumber, int pageSize);
    }
}