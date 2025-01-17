using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.ProposalDtos;

public class CreateProposalDto
{
    public string ClientId { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public ProposalStatus Status { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}