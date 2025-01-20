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
                    CompanyBusinessName = client?.BusinessInfo.TradeName ?? string.Empty,
                    Status = new ProposalStatusDto
                    {
                        Proposal = proposal.Status.Proposal,
                        Sending = proposal.Status.Sending,
                        Review = proposal.Status.Review
                    },
                    Sites = proposal.Sites.Select(s => new SiteDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Address = s.Address,
                        City = s.City,
                        Phone = s.Phone,
                        Wastes = s.Wastes.Select(w => new WasteDto
                        {
                            Type = w.Type,
                            Classification = w.Classification,
                            Treatment = w.Treatment,
                            Frequency = w.Frequency,
                            Price = w.Price
                        }).ToList()
                    }).ToList(),
                    History = proposal.History.Select(h => new ProposalHistoryDto
                    {
                        Date = h.Date,
                        PotentialClientId = h.PotentialClientId,
                        Action = h.Action
                    }).ToList(),
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