using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.ProposalDtos;

public class ProposalDto
{
    public string Id { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public ProposalStatus Status { get; set; } = new();
    public List<Site> Sites { get; set; } = new();
    public List<ProposalHistory> History { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CompanyBusinessName { get; set; } = string.Empty;
}