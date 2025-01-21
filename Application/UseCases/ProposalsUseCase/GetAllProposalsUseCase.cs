using MongoDB.Bson;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;
public class GetAllProposalsUseCase : IGetAllProposalsUseCase
{

    private readonly IPotentialClientRepository _clientRepository;
    private IProposalRepository _proposalrepository;


    public GetAllProposalsUseCase(IProposalRepository proposalrepository, IPotentialClientRepository clientRepository)
    {
        _proposalrepository = proposalrepository;
        _clientRepository = clientRepository;
    }

    public async Task<(List<ProposalDto>, int)> Execute(int pageNumber, int pageSize)
    {
        try
        {
            var (proposals, totalCount) = await _proposalrepository.GetAllProposals(pageNumber, pageSize);
            var proposalsDto = new List<ProposalDto>();

            foreach (var proposal in proposals)
            {
                var client = await _clientRepository.GetByIdPotencialClient(proposal.ClientId);

                var proposalDto = new ProposalDto
                {
                    Id = proposal.Id,
                    ClientId = proposal.ClientId,
                    Number = proposal.Number,
                    CompanyBusinessName = client?.BusinessInfo?.TradeName ?? string.Empty,
                    Status = new ProposalStatusDto
                    {
                        Proposal = proposal.Status?.Proposal ?? string.Empty,
                        Sending = proposal.Status?.Sending ?? string.Empty,
                        Review = proposal.Status?.Review ?? string.Empty
                    },
                    Sites = proposal.Sites?.Select(s => new SiteDto
                    {
                        Id = s.Id,
                        Name = s.Name ?? string.Empty,
                        Address = s.Address ?? string.Empty,
                        City = s.City ?? string.Empty,
                        Phone = s.Phone ?? string.Empty,
                        Wastes = s.Wastes?.Select(w => new WasteDto
                        {
                            Type = w.Type ?? string.Empty,
                            Classification = w.Classification ?? string.Empty,
                            Treatment = w.Treatment ?? string.Empty,
                            Price = w.Price
                        }).ToList() ?? new List<WasteDto>()
                    }).ToList() ?? new List<SiteDto>(),
                    History = proposal.History?.Select(h => new ProposalHistoryDto
                    {
                        Date = h.Date,
                        PotentialClientId = h.PotentialClientId ?? string.Empty,
                        Action = h.Action ?? string.Empty
                    }).ToList() ?? new List<ProposalHistoryDto>(),
                    CreatedAt = proposal.CreatedAt,
                    UpdatedAt = proposal.UpdatedAt

                };

                proposalsDto.Add(proposalDto);
            }

            return (proposalsDto, totalCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UseCase Error: {ex.Message}");
            throw;
        }
    }
}