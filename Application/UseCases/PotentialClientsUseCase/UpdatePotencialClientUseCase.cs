using System.Text.RegularExpressions;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;
public class UpdatePotentialClientUseCase : IUpdatePotentialClientUseCase
{
    private readonly IPotentialClientRepository _repository;
    private readonly string[] VALID_STATUSES = { "Activo", "Pendiente", "Inactivo" };

    public UpdatePotentialClientUseCase(IPotentialClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdatePotentialClientDto> Execute(string id, UpdatePotentialClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("El ID del cliente potencial es obligatorio.");

        var existingClient = await _repository.GetByIdPotencialClient(id)
            ?? throw new KeyNotFoundException($"Cliente potencial con ID {id} no encontrado.");

        await ValidateUpdate(dto, existingClient);

        var now = DateTime.UtcNow;
        var updatedClient = new PotentialClient
        {
            Id = id,
            Identification = dto.Identification ?? existingClient.Identification,
            BusinessInfo = dto.BusinessInfo ?? existingClient.BusinessInfo,
            Location = dto.Location ?? existingClient.Location,
            Status = new Status
            {
                Current = dto.Status ?? existingClient.Status.Current,
                History = new List<StatusHistory>(existingClient.Status.History)
            },
            CreatedAt = existingClient.CreatedAt,
            UpdatedAt = now
        };

        // Agregar nuevo estado al historial si cambió
        if (dto.Status != null && dto.Status != existingClient.Status.Current)
        {
            updatedClient.Status.History.Add(new StatusHistory
            {
                Status = dto.Status,
                Date = now,
                Observation = "Estado actualizado"
            });
        }

        await _repository.UpdatePotentialClient(id, updatedClient);

        return dto;
    }

    private async Task ValidateUpdate(UpdatePotentialClientDto dto, PotentialClient existingClient)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        // Validar nuevo estado si se proporciona
        if (dto.Status != null && !VALID_STATUSES.Contains(dto.Status))
            throw new ArgumentException($"Estado inválido. Debe ser uno de: {string.Join(", ", VALID_STATUSES)}");

        // Validar BusinessInfo si se actualiza
        if (dto.BusinessInfo != null)
        {
            if (!string.IsNullOrWhiteSpace(dto.BusinessInfo.Email) &&
                !Regex.IsMatch(dto.BusinessInfo.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Formato de email inválido");

            // Verificar duplicados solo si se cambia el email o nombre comercial
            var existingClients = await _repository.GetAllAsync(CancellationToken.None);

            if (!string.IsNullOrWhiteSpace(dto.BusinessInfo.Email) &&
                existingClients.Any(c => c.Id != existingClient.Id &&
                                       c.BusinessInfo.Email.Equals(dto.BusinessInfo.Email,
                                       StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Ya existe un cliente con este email");

            if (!string.IsNullOrWhiteSpace(dto.BusinessInfo.TradeName) &&
                existingClients.Any(c => c.Id != existingClient.Id &&
                                       c.BusinessInfo.TradeName.Equals(dto.BusinessInfo.TradeName,
                                       StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Ya existe un cliente con este nombre comercial");
        }
    }
}