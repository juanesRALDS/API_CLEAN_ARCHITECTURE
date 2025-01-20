using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

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
            throw new ArgumentException("El número de página debe ser mayor que 0.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("El tamaño de página debe ser mayor que 0.", nameof(pageSize));

        try
        {
            var clients = await _repository.GetAllPotentialClientsWithProposals(pageNumber, pageSize);

            return clients.Select(c => new PotentialClientDto
            {
                Id = c.Id,
                Identification = new IdentificationDto
                {
                    Type = c.Identification.Type,
                    Number = c.Identification.Number
                },
                BusinessInfo = new BusinessInfoDto
                {
                    
                    TradeName = c.BusinessInfo.TradeName,
                    EconomicActivity = c.BusinessInfo.EconomicActivity,
                    Email = c.BusinessInfo.Email,
                    Phone = c.BusinessInfo.Phone
                },
                Location = new LocationDto
                {
                    Address = c.Location.Address,
                    City = c.Location.City,
                    Department = c.Location.Department
                },
                Status = new StatusDto
                {
                    Current = c.Status.Current,
                    History = c.Status.History.Select(h => new StatusHistoryDto
                    {
                        Status = h.Status,
                        Date = h.Date,
                        Observation = h.Observation
                    }).ToList()
                },
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener clientes con propuestas", ex);
        }
    }
}