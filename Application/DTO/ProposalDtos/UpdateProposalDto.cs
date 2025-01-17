namespace SagaAserhi.Application.DTO.ProposalDtos;

public class UpdateProposalStatusDto
{
    public string Proposal { get; set; } = string.Empty;
    public string Sending { get; set; } = string.Empty;
    public string Review { get; set; } = string.Empty;
}

public class UpdateProposalDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public UpdateProposalStatusDto Status { get; set; } = new();
}