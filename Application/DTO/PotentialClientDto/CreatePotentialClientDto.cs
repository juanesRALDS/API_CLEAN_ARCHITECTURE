using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.PotentialClientDto;

public class CreatePotentialClientDto
{
    public Identification Identification { get; set; } = new();
    public string LegalRepresentative { get; set; } = string.Empty;
    public BusinessInfo BusinessInfo { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}