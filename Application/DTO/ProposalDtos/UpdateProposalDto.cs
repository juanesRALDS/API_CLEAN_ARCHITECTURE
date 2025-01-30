namespace SagaAserhi.Application.DTO.ProposalDtos;

public class UpdateProposalStatusDto
{
    public string Proposal { get; set; } = string.Empty;
    public string Sending { get; set; } = string.Empty;
    public string Review { get; set; } = string.Empty;
}

public class UpdateProposalDto
{
    public UpdateProposalStatusDto Status { get; set; } = new();
}