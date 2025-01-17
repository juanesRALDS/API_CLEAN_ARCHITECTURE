using Microsoft.IdentityModel.Tokens;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;

public class AddProposalToPotentialClientUseCase : IAddProposalToPotentialClientUseCase
{
    private readonly IPotentialClientRepository _potentialClientRepository;
    private readonly IProposalRepository _proposalRepository;

    public AddProposalToPotentialClientUseCase(
        IPotentialClientRepository potentialClientRepository,
        IProposalRepository proposalRepository)
    {
        _potentialClientRepository = potentialClientRepository;
        _proposalRepository = proposalRepository;
    }

    public async Task<string> Execute(string clientId, CreateProposalDto proposalDto)
    {
        if (string.IsNullOrWhiteSpace(clientId))
            throw new ArgumentException("El ID del cliente es requerido");

        if (proposalDto == null)
            throw new ArgumentNullException(nameof(proposalDto));

        var client = await _potentialClientRepository.GetByIdPotencialClient(clientId)
            ?? throw new InvalidOperationException($"No se encontró el cliente con ID: {clientId}");

        var proposal = new Proposal
        {
            ClientId = clientId,
            Number = GenerateProposalNumber(),
            Status = new ProposalStatus
            {
                Proposal = "Pendiente",
                Sending = "No enviado",
                Review = "Sin revisar"
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            History = new List<ProposalHistory>
            {
                new() {
                    Action = "Creación de propuesta",
                    Date = DateTime.UtcNow,
                    PotentialClientId = clientId
                }
            }
        };

        var success = await _proposalRepository.CreateProposal(proposal);
        if (!success)
            throw new InvalidOperationException("No se pudo crear la propuesta");

        return "Propuesta creada exitosamente";
    }

    private string GenerateProposalNumber()
    {
        return $"PROP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}";
    }
}