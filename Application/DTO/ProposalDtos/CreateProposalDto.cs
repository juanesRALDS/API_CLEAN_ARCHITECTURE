namespace SagaAserhi.Application.DTO.ProposalDtos;

public class CreateProposalDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}