using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Proposal.UseCaseProposal;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;

public class GetAllProposalsUseCase : IGetAllProposalsUseCase
{
    private readonly IProposalRepository _repository;

    public GetAllProposalsUseCase(IProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProposalDto>> Execute(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        try
        {
            List<Domain.Entities.Proposal>? proposals = await _repository.GetAllProposals(pageNumber, pageSize);
            return proposals.Select(p => new ProposalDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Amount = p.Amount,
                Status = p.Status,
                CreationDate = p.CreationDate,
                PotentialClientId = p.PotentialClientId,
                CompanyBusinessName = p.CompanyBusinessName
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener propuestas: {ex.Message}", ex);
        }
    }
}