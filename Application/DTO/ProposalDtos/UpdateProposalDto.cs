namespace SagaAserhi.Application.DTO.ProposalDtos;

public class UpdateProposalDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PotentialClientId { get; set; } = string.Empty;
}