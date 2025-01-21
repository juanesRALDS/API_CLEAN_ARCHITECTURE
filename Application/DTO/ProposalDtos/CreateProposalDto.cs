using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.ProposalDtos;

public class CreateProposalDto
{
    public ProposalStatus Status { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}