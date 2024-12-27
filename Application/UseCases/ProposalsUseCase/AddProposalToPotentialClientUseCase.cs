using Microsoft.IdentityModel.Tokens;
using SagaAserhi.Application.DTO;
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
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Client Id is required");
        }
        if (clientId == null)
        {
            throw new ArgumentNullException(nameof(clientId));
        }

        var proposal = new Proposal
        {
            Title = proposalDto.Title,
            Description = proposalDto.Description,
            Amount = proposalDto.Amount,
            Status = proposalDto.Status,
            PotentialClientId = clientId
        };

        var result = await _potentialClientRepository.AddProposalToPotentialClient(clientId, proposal);

        if (!result)
        {
            throw new SecurityTokenException("Proposal could not be added to the potential client");
        }
        
        return "proposal added to the potential client";
        
    }
}