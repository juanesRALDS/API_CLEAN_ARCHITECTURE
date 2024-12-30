using Microsoft.IdentityModel.Tokens;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCa;


public class AddProposalToPotentialClientUseCase : IAddProposalToPotentialClientUseCase
{
    private readonly IPotentialClientRepository _potentialClientRepository;

    public AddProposalToPotentialClientUseCase(IPotentialClientRepository potentialClientRepository)
    {
        _potentialClientRepository = potentialClientRepository;
    }

    public async Task<string> Execute(string clientId, CreateProposalDto proposalDto)
    {
        try
        {
            // Validar ID del cliente
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("El ID del cliente es requerido");

            // Validar DTO
            if (proposalDto == null)
                throw new ArgumentNullException(nameof(proposalDto));

            // Verificar si existe el cliente
            var client = await _potentialClientRepository.GetByIdPotencialClient(clientId);
            if (client == null)
                throw new InvalidOperationException($"No se encontró el cliente con ID: {clientId}");

            // Crear nueva propuesta
            var proposal = new Proposal
            {
                Title = proposalDto.Title,
                Description = proposalDto.Description,
                Amount = proposalDto.Amount,
                Status = "Pendiente",
                CreationDate = DateTime.UtcNow,
                PotentialClientId = clientId
            };

            // Intentar agregar la propuesta
            var result = await _potentialClientRepository.AddProposalToPotentialClient(clientId, proposal);

            if (!result)
                throw new InvalidOperationException("No se pudo agregar la propuesta al cliente");

            return "Propuesta agregada exitosamente";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al agregar la propuesta: {ex.Message}", ex);
        }
    }
}
