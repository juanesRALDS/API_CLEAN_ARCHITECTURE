using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

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

        try
        {
            List<PotentialClient>? clients = await _repository.GetAllPotentialClientsWithProposals(pageNumber, pageSize);

            return clients.Select(c => new PotentialClientDto
            {
                Id = c.Id.ToString(),

                CompanyBusinessName = c.CompanyBusinessName,
                ContactPhone = c.ContactPhone,
                ContactEmail = c.ContactEmail,
                Proposals = c.Proposals?.Select(proposalID => new ProposalDto
                {
                    Id = proposalID, 
                    CompanyBusinessName = c.CompanyBusinessName
                }).ToList() ?? new List<ProposalDto>()
            }).ToList();

        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener clientes con propuestas", ex);
        }
    }
}