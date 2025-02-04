using System.Text.RegularExpressions;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;
public class CreatePotentialClientUseCase : ICreatePotentialClientUseCase
{
    private readonly IPotentialClientRepository _repository;
    private readonly string[] VALID_STATUSES = { "Active", "Pending", "Inactive" };

    public CreatePotentialClientUseCase(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Execute(CreatePotentialClientDto dto)
    {
        await ValidateInput(dto);

        DateTime now = DateTime.UtcNow;
        PotentialClient potentialClient = new()
        {
            Identification = dto.Identification,
            LegalRepresentative = dto.LegalRepresentative,
            BusinessInfo = dto.BusinessInfo,
            Status = new Status
            {
                Current = dto.Status,
                History = new List<StatusHistory>
                {
                    new()
                    {
                        Status = dto.Status,
                        Date = now,
                        Observation = "Cliente potencial creado"
                    }
                }
            },
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repository.CreatePotentialClient(potentialClient);
        return "Cliente potencial creado exitosamente";
    }


    private async Task ValidateInput(CreatePotentialClientDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        // Validar Identification
        if (string.IsNullOrWhiteSpace(dto.Identification.Type))
            throw new ArgumentException("El tipo de identificación no puede estar vacío", nameof(dto.Identification.Type));
        if (string.IsNullOrWhiteSpace(dto.Identification.Number))
            throw new ArgumentException("El número de identificación no puede estar vacío", nameof(dto.Identification.Number));
        if (string.IsNullOrWhiteSpace(dto.LegalRepresentative))
            throw new ArgumentException("el representate legal no puede estar vacio", nameof(dto.LegalRepresentative));
        // Validar BusinessInfo
        if (string.IsNullOrWhiteSpace(dto.BusinessInfo.TradeName))
            throw new ArgumentException("El nombre comercial no puede estar vacío", nameof(dto.BusinessInfo.TradeName));
        if (string.IsNullOrWhiteSpace(dto.BusinessInfo.EconomicActivity))
            throw new ArgumentException("La actividad económica no puede estar vacía", nameof(dto.BusinessInfo.EconomicActivity));
        if (string.IsNullOrWhiteSpace(dto.BusinessInfo.Email))
            throw new ArgumentException("El email no puede estar vacío", nameof(dto.BusinessInfo.Email));
        if (!Regex.IsMatch(dto.BusinessInfo.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("El formato del email no es válido", nameof(dto.BusinessInfo.Email));
        if (string.IsNullOrWhiteSpace(dto.BusinessInfo.Phone))
            throw new ArgumentException("El teléfono no puede estar vacío", nameof(dto.BusinessInfo.Phone));


        // Validar Status
        if (string.IsNullOrWhiteSpace(dto.Status) || !VALID_STATUSES.Contains(dto.Status))
            throw new ArgumentException($"El estado debe ser uno de los siguientes: {string.Join(", ", VALID_STATUSES)}", nameof(dto.Status));

        // Verificar duplicados
        IEnumerable<PotentialClient>? existingClients = await _repository.GetAllAsync(CancellationToken.None);

        if (existingClients.Any(c => c.Identification.Number.Equals(dto.Identification.Number, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ya existe un cliente con este número de identificación");

        if (existingClients.Any(c => c.BusinessInfo.Email.Equals(dto.BusinessInfo.Email, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ya existe un cliente con este email");

        if (existingClients.Any(c => c.BusinessInfo.TradeName.Equals(dto.BusinessInfo.TradeName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Ya existe un cliente con este nombre comercial");
    }
}