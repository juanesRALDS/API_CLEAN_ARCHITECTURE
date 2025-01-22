using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;

namespace SagaAserhi.Application.UseCases.ContractUseCase;

public class GetAllContractsUseCase : IGetAllContractsUseCase
{
    private readonly IContractRepository _contractRepository;

    public GetAllContractsUseCase(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<(List<ContractDto>, int)> Execute(int pageNumber, int pageSize)
    {
        try
        {
            var (contracts, totalCount) = await _contractRepository.GetAllContracts(pageNumber, pageSize);

            var contractsDto = contracts.Select(c => new ContractDto
            {
                Id = c.Id,
                ProposalId = c.ProposalId,
                ClientId = c.ClientId,
                Number = c.Number,
                Status = c.Status,
                Dates = new ContractDatesDto
                {
                    Start = c.Dates.Start,
                    End = c.Dates.End
                },
                Documents = new DocumentsDto
                {
                    Annexes = c.Documents.Annexes.Select(a => new AnnexDto
                    {
                        Title = a.Title,
                        Path = a.Path,
                        UploadDate = a.UploadDate
                    }).ToList(),
                    Clauses = c.Documents.Clauses.Select(cl => new ClauseDto
                    {
                        Title = cl.Title,
                        Content = cl.Content
                    }).ToList()
                },
                History = c.History.Select(h => new ContractHistoryDto
                {
                    Status = h.Status,
                    Date = h.Date,
                    Observation = h.Observation
                }).ToList(),
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return (contractsDto, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener contratos: {ex.Message}", ex);
        }
    }
}