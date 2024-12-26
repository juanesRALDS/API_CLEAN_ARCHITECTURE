using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Infrastructure.Repositories;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

public class GetAllPotentialClientsWithProposalsUseCase : IGetAllPotentialClientsWithProposalsUseCase
{
    private readonly IPotentialClientRepository _repository;

    public GetAllPotentialClientsWithProposalsUseCase(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PotentialClientDto>> Execute(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        var clients = await _repository.GetAllPotentialClientsWithProposals(pageNumber, pageSize);
        return clients.Select(c => new PotentialClientDto
        {
            Id = c.Id,
            PersonType = c.PersonType,
            CompanyBusinessName = c.CompanyBusinessName,
            RepresentativeNames = c.RepresentativeNames,
            RepresentativeLastNames = c.RepresentativeLastNames,
            ContactPhone = c.ContactPhone,
            ContactEmail = c.ContactEmail,
            Proposals = c.Proposals?.Select(p => new ProposalDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Amount = p.Amount,
                Status = p.Status,
                CreationDate = p.CreationDate,
                PotentialClientId = p.PotentialClientId
            }).ToList() ?? new List<ProposalDto>()
        }).ToList();
    }
}