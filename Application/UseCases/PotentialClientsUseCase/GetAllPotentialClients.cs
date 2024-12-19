using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Infrastructure.Repositories;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

public class GetAllPotentialClientsUseCase : IGetAllPotentialClientsUseCase
{
    private readonly IPotentialClientRepository _repository;

    public GetAllPotentialClientsUseCase(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PotentialClientDto>> Execute(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        var clients = await _repository.GetAllPotentialClients(pageNumber, pageSize);
        return clients.Select(c => new PotentialClientDto
        {
            IdentificationTypeId = c.IdentificationTypeId,
            EconomicActivityId = c.EconomicActivityId,
            CreationDate = c.CreationDate,
            PersonType = c.PersonType,
            PotentialClientSize = c.PotentialClientSize,
            CompanyBusinessName = c.CompanyBusinessName,
            RepresentativeNames = c.RepresentativeNames,
            RepresentativeLastNames = c.RepresentativeLastNames,
            RepresentativeIdentification = c.RepresentativeIdentification,
            ContactPhone = c.ContactPhone,
            ContactEmail = c.ContactEmail
        }).ToList();
    }
}