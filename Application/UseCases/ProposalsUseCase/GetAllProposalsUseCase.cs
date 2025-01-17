using MongoDB.Bson;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;
public class GetAllProposalsUseCase : IGetAllProposalsUseCase
{
    private readonly IProposalRepository _repository;

    public GetAllProposalsUseCase(IProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<(List<ProposalDto> Proposals, int TotalCount)> Execute(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("El número de página debe ser mayor a 0", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("El tamaño de página debe ser mayor a 0", nameof(pageSize));

        try
        {
            var (proposals, totalCount) = await _repository.GetAllProposals(pageNumber, pageSize);

            var proposalsDto = proposals.Select(p => new ProposalDto
            {
                Id = p.Id,
                ClientId = p.ClientId,
                Number = p.Number,
                Status = p.Status,
                Sites = p.Sites,
                History = p.History,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                CompanyBusinessName = p.PotentialClient?.BusinessInfo.TradeName ?? string.Empty
            }).ToList();

            return (proposalsDto, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener propuestas: {ex.Message}", ex);
        }
    }
}